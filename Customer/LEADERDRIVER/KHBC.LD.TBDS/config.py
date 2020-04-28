
#mysql数据库配置
DB_CONFIG={
    "host":"localhost",
    "port":3306,
    "db":"test",
    "user":"root",
    "password":"123456",
    "charset":"utf8"
}

#刀补相关刀具信息表
cutter_compensation_table="cutter_compensation"

#主轴刀具负载表
cutter_load="cutter_load"

#断刀检测相关刀具信息表
cutter_detection_table="cutter_detection"

#采样周期（ms）
sampling_period=0.01
#积分上限系数
up_ratio=1.2
#积分下限系数
low_ratio=0.8
#微分上限系数
dp_ratio=0.95

#三坐标检测结果表
derection_result="derection"


#刀具与检测参数关联配置
tools=[
#1
    {
        "tool_type":1,
        "Dimension_id":[#该刀具关联的零件部位
            "Loc1"
        ],
        "name":[#部位对应的尺寸名
            {
                "X":["X"]#尺寸需要刀具往哪个方向进行补偿
            }
        ]
     },
#2
    {
        "tool_type":2,
        "Dimension_id":[
            "LOC1",
            "LOC2"
        ],
        "name":[
            {
                "X":["X"]
            },
            {
                "Y":["Y"]
            }
        ]
     },
#3
    {
        "tool_type":3,
        "Dimension_id":[
            "LOC1",
            "LOC2"
        ],
        "name":[
            {
                "X":["X"]
            },
            {
                "Y":["Y"]
            }
        ]
     },
#4
    {
        "tool_type":4,
        "Dimension_id":[
            "LOC1",
            "LOC2"
        ],
        "name":[
            {
                "X":["X"]
            },
            {
                "Y":["Y"]
            }
        ]
     },
]