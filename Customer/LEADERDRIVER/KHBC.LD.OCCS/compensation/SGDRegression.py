import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.linear_model import SGDRegressor
from sklearn import metrics
import matplotlib.pyplot as plt


#线性回归-梯度下降
class SGD:

    def __init__(self,sample_feature,sample_output):
        """
        :param sample_feature: 样本特征 list
        :param sample_output: 样本输出 list
        :param condition_dict: 数据库查询条件 dict
        """
        self.sample_feature=sample_feature
        self.sample_out=sample_output

        #获得一个线性回归模型
        self.estimator=SGDRegressor()

    def train(self,result):
        '''
        训练模型
        :param result:
        :return:
        '''
        self.result=result
        x=result[self.sample_feature]
        y=result[self.sample_out]
        print(type(x))
        #划分数据集
        self.x_train,self.x_test,self.y_train,self.y_test=train_test_split(x,y,test_size=0.2,random_state=0)
        #对模型传入输入数据和输出数据
        self.estimator.fit(self.x_train,self.y_train)

    def output_model_params(self):
        '''
        打印训练后模型的权重系数和偏置
        :return:
        '''
        print("梯度下降-权重系数为：",self.estimator.coef_)
        print("梯度下降-偏置为：",self.estimator.intercept_)

    def predict(self,input_list):
        """
        输入特征预测结果
        :param input_list: 特征值 list
        :return: 预测值
        """
        input_list=pd.DataFrame(input_list)
        result=self.estimator.predict((input_list))
        return result

    def draw(self):
        x=np.arange(0,2000,7)
        y= self.estimator.coef_[0] * x + self.estimator.intercept_[0]
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
