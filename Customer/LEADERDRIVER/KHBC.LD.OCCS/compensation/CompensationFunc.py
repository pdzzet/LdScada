from DBController.DBHelper import DBHelper
import config
import pandas as pd
from compensation.RidgeRegression import RidgeR
import redis

class Compensation:

    def __init__(self,tool_id):
        self.tool_id=tool_id
        self.db=DBHelper()
        self.tool=None
        self.tool_type=None
        self.dimension_id=[]
        self.dimension_name=[]

    def get_compensation(self,for_now):
        self.tool=self.db.get_some(['*'],config.cutter_compensation_table,{"tool_id":1})
        self.tool_type=self.tool[0][1]
        self.dimension_id=config.tools[self.tool_type-1]["Dimension_id"]
        self.dimension_name=config.tools[self.tool_type-1]["name"]
        result=[]
        for i in range(len(self.dimension_id)):
            for j in self.dimension_name[i]:
                sql="select * from {cutter}" \
                    " INNER JOIN {derection} " \
                    "ON {cutter}.component_id={derection}.component_id " \
                    "where {cutter}.tool_type={tool_type} " \
                    "AND {derection}.dimension_id='{dimension_id}' " \
                    "AND {derection}.name='{dimension_name}'".format(cutter=config.cutter_compensation_table,derection=config.derection_result,tool_type=self.tool_type,dimension_id=self.dimension_id[i],dimension_name=j)
                result=pd.read_sql(sql,self.db.con())
        sample_feature=['machine_time']
        sample_output=['compensation_value']
        LR=RidgeR(sample_feature, sample_output)
        LR.train(result)
        print("MSE:",LR.evaluation_model())
        LR.draw()
        LR.output_model_params()
        return LR.predict(for_now)


    def set_compensation(self,tool_id):
        r=redis.Redis(host='127.0.0.1',port=6379)
        r.lpush("set_compensation",tool_id)

    def __del__(self):
        self.db.close_db()
