import threading
from DBController.DBHelper import DBHelper
from config import cutter_load,sampling_period
import math

class DetectionPattern(threading.Thread):
    def __init__(self,cutter_info_dict,tool_type):
        threading.Thread.__init__(self)
        self.cutter_info_dict = cutter_info_dict
        self.tool_type = tool_type
        self.last_electric=-1
        self.N_act=0
        self.W_act=0
        self.db=DBHelper()

    def run(self):
        load_data=self.db.get_one(["WUp","WLow","DUp","NLimit"],cutter_load,{"ToolType":self.tool_type})
        w_up=load_data[0]
        w_low=load_data[1]
        D_up=load_data[2]
        n_limit=load_data[3]
        while True:
            if self.cutter_info_dict[self.tool_type]:
                cutter_info_list = self.get_one_data()
                print("get data :",cutter_info_list)
                # 主轴停止旋转
                if cutter_info_list[3] == 0:
                    # 刀具断刀
                    if self.W_act < w_low:
                        self.db.update(cutter_load, {"TippingAlarm": 1})
                        print("刀具断刀")
                        break
                    self.del_data_vessel()
                    print("运行正常")
                    break
                if self.last_electric == -1:
                    self.last_electric = cutter_info_list[2]
                    continue
                dP = math.fabs(cutter_info_list[2] - self.last_electric)
                self.W_act+=cutter_info_list[2] * sampling_period
                if dP>D_up:
                    self.N_act+=1
                #刀具磨损
                if self.W_act>w_up:
                    self.db.update(cutter_load,{"WearWarning":1})
                    print("刀具磨损")
                    self.del_data_vessel()
                    break
                #刀具崩刃
                if self.N_act>n_limit:
                    self.db.update(cutter_load,{"CuttingAlarm":1})
                    print("刀具崩刃")
                    self.del_data_vessel()
                    break

    # queue队列中取数据
    def get_one_data(self):
        return self.cutter_info_dict[self.tool_type].get()

    # 检测结束时删除对应缓存抓取到的刀具数据的字典的键值对
    def del_data_vessel(self):
        del self.cutter_info_dict[self.tool_type]