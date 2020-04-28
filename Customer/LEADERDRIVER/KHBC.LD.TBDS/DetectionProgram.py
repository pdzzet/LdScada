import time
from DBController.DBHelper import DBHelper
from config import cutter_detection_table,cutter_load
from detection.CutterLearn import LearnPattern
from detection.BreakageDetection import DetectionPattern
import queue

cutter_info_dict = {}

while(True):
    db=DBHelper()
    field_list=["ToolId","ToolType","SpindleElectric","SpindleSpeed","id"]
    wait_for_learn=db.get_some(field_list,cutter_detection_table,{"IsRead":0})
    if wait_for_learn:
        for i in wait_for_learn:
            #刀具没有开始使用的状态（不做记录）
            if i[3]==0:
                if i[1] not in cutter_info_dict:
                    continue
            print("暂未放入queue的数据ToolType:",i[1])
            if i[1] in cutter_info_dict:
                print("放入queue数据：",i)
                #往对应的字典键对应的列表增加数据，供线程学习或监测
                cutter_info_dict[i[1]].put(list(i))
            else:
                #开始学习或检测
                cutter_info_dict[i[1]]=queue.Queue()
                is_learn_data=db.get_one(['Learned'],cutter_load,{"ToolType":i[1]})
                if (is_learn_data if is_learn_data == None else is_learn_data[0]):
                    #监测
                    print("开始检测")
                    detection_thread=DetectionPattern(cutter_info_dict,i[1])
                    detection_thread.start()
                else:
                    #学习
                    print("开始学习")
                    learn_thread=LearnPattern(cutter_info_dict,i[1])
                    learn_thread.start()
            db.update(cutter_detection_table,{"IsRead":1},{"id":i[4]})

    time.sleep(1)

