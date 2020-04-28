from config import cutter_compensation_table
from DBController.DBHelper import DBHelper
import time
from compensation.CompensationFunc import Compensation

db=DBHelper()

while True:
    wait_for_learn=db.get_some(["*"],cutter_compensation_table,{"IsRead":0})

    if wait_for_learn:
        for i in wait_for_learn:
            db.update(cutter_compensation_table,{"IsRead":1},{"id":i[0]})
            compensation=Compensation(i[2])
            compensation_value=compensation.get_compensation(i[4])
            compensation.set_compensation(compensation_value)
    time.sleep(1)