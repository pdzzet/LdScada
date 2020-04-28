import pandas as pd
import numpy as np
from DBController.DBHelper import DBHelper
from sklearn.model_selection import train_test_split
from sklearn.linear_model import Ridge,RidgeCV
from sklearn import metrics
import matplotlib.pyplot as plt

#岭回归
class RidgeR:

    def __init__(self,sample_feature,sample_output,condition_dict=None):
        """
        :param sample_feature: 样本特征 list
        :param sample_output: 样本输出 list
        :param condition_dict: 数据库查询条件 dict
        """
        self.db=DBHelper()
        self.sample_feature=sample_feature
        self.sample_out=sample_output
        self.condition_dict=condition_dict
        self.field=",".join(sample_feature+sample_output)

        from config import cutter_compensation_table
        #需要学习的表的name
        self.table=cutter_compensation_table

        # 构造不同的lambda值
        Lambdas = np.logspace(-5, 2, 200)
        self.ridge_cv = RidgeCV(alphas=Lambdas, normalize=True, scoring='neg_mean_squared_error', cv=10)



    def train(self,result):
        '''
        训练模型
        :param result:
        :return:
        '''
        self.result=result
        x=result[self.sample_feature]
        y=result[self.sample_out]
        #划分数据集
        self.x_train,self.x_test,self.y_train,self.y_test=train_test_split(x,y,test_size=0.2,random_state=0)
        # #标准化
        # self.transfer=StandardScaler()
        # self.x_train=self.transfer.fit_transform(self.x_train)
        # self.x_test=self.transfer.transform(self.x_test)

        # 对交叉验证模型传入输入数据和输出数据
        self.ridge_cv.fit(self.x_train, self.y_train)
        # 基于最佳lambda值建模
        self.estimator = Ridge(alpha=self.ridge_cv.alpha_)
        self.estimator.fit(self.x_train,self.y_train)

    def output_model_params(self):
        '''
        打印训练后模型的权重系数和偏置
        :return:
        '''
        print("岭回归-权重系数为：",self.estimator.coef_)
        print("岭回归-偏置为：",self.estimator.intercept_)

    def predict(self,input_list):
        """
        输入特征预测结果
        :param input_list: 特征值 list
        :return: 预测值
        """
        input_list = pd.DataFrame(input_list)
        # 标准化时使用这条
        # input_list=self.transfer.transform(input_list)
        result = self.estimator.predict((input_list))
        return result

    def draw(self):
        x=np.arange(0,2000,7)
        y= self.estimator.coef_[0][0] * x + self.estimator.intercept_[0]
        plt.plot(x,y)
        a=self.result[self.sample_feature]
        b=self.result[self.sample_out]
        plt.scatter(a,b)
        plt.show()

    def evaluation_model(self):
        '''
        模型评估
        :return: 均方误差
        '''
        y_pred=self.estimator.predict(self.x_test)
        #均方误差
        MSE=metrics.mean_squared_error(self.y_test,y_pred)
        return MSE

    def __del__(self):
        self.db.close_db()