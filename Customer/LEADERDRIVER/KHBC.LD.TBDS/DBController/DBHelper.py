import pymysql
from config import DB_CONFIG


class DBHelper(object):

    def __init__(self):
        self._conn = pymysql.connect(host=DB_CONFIG["host"],
                                     port=DB_CONFIG["port"],
                                     database=DB_CONFIG["db"],
                                     user=DB_CONFIG["user"],
                                     password=DB_CONFIG["password"],
                                     charset=DB_CONFIG["charset"]
                                     )
        self.__cursor = self._conn.cursor()

    def con(self):
        return self._conn

    def close_db(self):
        self.__cursor.close()
        self._conn.close()

    def get_some(self,fileds_list,table,condition_dict=None):
        """
        :param fileds_list:
        :param table:
        :param condition_dict:
        :return: 查询结果 元组
        """
        try:
            fileds=self._deal_values(fileds_list)

            sql="select {fields} from {table}".format(fields=fileds,table=table)
            if condition_dict:
                conditions = " AND ".join(i for i in self._deal_values(condition_dict))
                sql+=" where {condition}".format(condition=conditions)
            # print(sql)
            self.__cursor.execute(sql)
            result=self.__cursor.fetchall()
            if result:
                return result
            else:
                return None
        except Exception as e:
            print(e)

    def get_one(self,fileds_list,table,condition_dict=None):
        """
        :param fileds_list:
        :param table:
        :param condition_dict:
        :return: 查询结果 元组
        """
        try:
            fileds=self._deal_values(fileds_list)

            sql="select {fields} from {table}".format(fields=fileds,table=table)
            if condition_dict:
                conditions = " AND ".join(i for i in self._deal_values(condition_dict))
                sql+=" where {condition}".format(condition=conditions)
            # print(sql)
            self.__cursor.execute(sql)
            result=self.__cursor.fetchone()
            if result:
                return result
            else:
                return None
        except Exception as e:
            print(e)

    def row_count(self,fileds_list,table,condition_dict=None):
        """
        :param fileds_list:
        :param table:
        :param condition_dict:
        :return: 查询到的数据条数
        """
        try:
            fileds = self._deal_values(fileds_list)

            sql = "select {fields} from {table}".format(fields=fileds, table=table)
            if condition_dict:
                conditions = " AND ".join(i for i in self._deal_values(condition_dict))
                sql += " where {condition}".format(condition=conditions)
            # print(sql)
            self.__cursor.execute(sql)
            result = self.__cursor.rowcount
            return result
        except Exception as e:
            print(e)

    def insert(self, table, insert_data):
        """
        :param table:
        :param insert_data:
        :return:
        """
        try:
            keys=[]
            values=[]
            for key,value in insert_data.items():
                keys.append(key)
                values.append(self._deal_values(value))
            keys=",".join(str(i) for i in keys)
            values=",".join(str(i) for i in values)
            sql = "insert into {table}({key}) values ({val})".format(table=table, key=keys, val=values)
            effect_row = self.__cursor.execute(sql)
            self._conn.commit()
            return effect_row
        except Exception as e:
            print(e)

    def delete(self, table, condition):
        """
        :param table:
        :param condition:
        :return:
        """
        condition_list = self._deal_values(condition)
        condition_data=" and ".join(condition_list)
        sql = "delete from {table} where {condition}".format(table=table, condition=condition_data)
        effect_row = self.__cursor.execute(sql)
        self._conn.commit()
        # self.close_db()
        return effect_row

    def update(self, table, data, condition=None):
        """
        :param table:
        :param data:
        :param condition:
        :return:
        """
        update_list = self._deal_values(data)
        update_data = ",".join(update_list)
        if condition is not None:
            condition_list = self._deal_values(condition)
            condition_data = ' and '.join(condition_list)
            sql = "update {table} set {values} where {condition}".format(table=table, values=update_data,
                                                                         condition=condition_data)
        else:
            sql = "update {table} set {values}".format(table=table, values=update_data)
        effect_row = self.__cursor.execute(sql)
        self._conn.commit()
        # self.close_db()
        return effect_row

    def query_sql(self, sql):
        # print(sql)
        self.__cursor.execute(sql)
        result = self.__cursor.fetchall()
        if result:
            return result
        else:
            return None

    def _deal_values(self, value):
        """
        self._deal_values(value) -> str or list
            处理传进来的参数
        """
        # 如果是字符串则加上''
        if isinstance(value, str):
            value = ("'{value}'".format(value=value))
        # 如果是字典则变成key=value形式
        elif isinstance(value, dict):
            result=[]
            for key, value in value.items():
                value = self._deal_values(value)
                res = "{key}={value}".format(key=key, value=value)
                result.append(res)
            return result
        #如果是列表则将其变成字符串并在每个值间加上','
        elif isinstance(value,list):
            result=",".join(str(i) for i in value)
            return result
        else:
            value = (str(value))
        return value