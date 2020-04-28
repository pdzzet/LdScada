from config import sampling_period,up_ratio,low_ratio,dp_ratio,cutter_load
import math
from DBController.DBHelper import DBHelper
import threading


class LearnPattern(threading.Thread):
    def __init__(self,cutter_info_dict,tool_type):
        threading.Thread.__init__(self)
        self.cutter_info_dict=cutter_info_dict
        self.tool_type=tool_type
        self.W_act=0
        self.max_dP=0
        self.last_electric=-1
        self.db=DBHelper()
        self.n_limit=0

    def run(self):
        while(True):
            if self.cutter_info_dict[self.tool_type]:
                self.data=self.db.get_one(["*"],cutter_load,{"ToolType":self.tool_type})
                self.cutter_info_list = self.get_one_data()
                print("get data :",self.cutter_info_list)
                #已学习过第一次
                if self.data:
                    if self.learn_nlimit()==1:
                        self.del_data_vessel()
                        break
                #第一次学习
                else:
                    if self.learn_wup_wlow_dup()==1:
                        self.del_data_vessel()
                        break

    #计算能耗积分和导数上限
    def compute_wact_maxdp(self):
        # self.cutter_info_list = self.get_one_data()
        if self.last_electric == -1:
            self.last_electric = self.cutter_info_list[2]
            return
        self.W_act += self.cutter_info_list[2] * sampling_period
        dP = math.fabs(self.cutter_info_list[2] - self.last_electric)
        self.last_electric = self.cutter_info_list[2]
        if dP > self.max_dP:
            self.max_dP = dP

    #第一次学习得到能耗上限、能耗下限、导数上限
    def learn_wup_wlow_dup(self):
        # 主轴停止旋转
        if self.cutter_info_list[3] == 0:
            W_up = self.W_act * up_ratio
            W_low = self.W_act * low_ratio
            D_up = self.max_dP * dp_ratio
            self.db.insert(cutter_load, {"ToolType": self.tool_type, "WUp": W_up, "WLow": W_low, "DUp": D_up})
            return 1
        self.compute_wact_maxdp()
        return 0

    #第二次学习通过上一次学习得出的导数上限和当前每次导数比较，得出导数超限次数
    def learn_nlimit(self):
        # 主轴停止旋转
        if self.cutter_info_list[3] == 0:
            self.db.update(cutter_load, {"Learned": 1, "Nlimit": self.n_limit})
            return 1
        # cutter_info_list = self.get_one_data()
        if self.last_electric == -1:
            self.last_electric = self.cutter_info_list[2]
            return 0
        dP = math.fabs(self.cutter_info_list[2] - self.last_electric)
        if dP > self.data[5]:
            print(dP,self.data[5])
            self.n_limit += 1
        return 0

    #queue队列中取数据
    def get_one_data(self):
        return self.cutter_info_dict[self.tool_type].get()

    #学习结束时删除对应缓存抓取到的刀具数据的字典的键值对
    def del_data_vessel(self):
        del self.cutter_info_dict[self.tool_type]

