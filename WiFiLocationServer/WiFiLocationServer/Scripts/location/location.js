$('#locationShow').click(function () {
    var data = getSelectData();
    $.ajax({
        url: "/api/LocationLog/?room=" + data.room + "&mobile=" + data.mobile + "&algorithm=" + data.algorithm,
        success: function (data) {
            if (data && data.hasOwnProperty('ds') && data.ds.length > 0 && data.hasOwnProperty('ds1') && data.ds.length > 0){
                generatePicture(data);
            }
            else {
                alert("数据库中无数据！");
            }
        },
        error: function (data) {
            alert(data.error);
        }
    });
});

function getSelectData() {
    var data = {};
    data.room = $('#room').val();
    data.mobile = $('#mobile').val();
    data.algorithm = $('#algorithm').val();
    return data;
}

function convertData(data) {
    var result = [];
    var log = data.ds;
    var coordList = data.ds1;
   result[0] =  log.map(function (item) {
       return item.actual_coord.split(',');
   });
    result[1] = log.map(function (item) {
        return item.location_coord.split(',');
    });
    result[2] = result[0].map(function (item,index) {
        var location = result[1][index];
        return Math.sqrt((item[0] - location[0])*(item[0] - location[0]) + (item[1] - location[1])*(item[1] - location[1])).toFixed(2);
    });
    result[1] = result[1].map(function (item, index) {
        item.push(result[2][index]);
        return item;
    });
    result[0] = result[0].map(function (item, index) {
        item.push(result[2][index]);
        return item;
    })
    result[3] = coordList.map(function (item) {
        return item.coord.split('，');
    });
    return result;
}

function generatePicture(data) {
    // 组织数据
    var dataList = convertData(data);

    $('#position,#deviation').css('display', 'block');

    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('position'));
    var myChart2 = echarts.init(document.getElementById('deviation'));

    // 指定图表的配置项和数据
    var option1 = {
        title: {
            text: '参考点与定位坐标点分布图'
        },
        tooltip: {},
        grid:{
            left: "10%",
            right: "10%",
            top: "10%",
            bottom:"10%"
        },
        legend: {
            data: ['参考点','定位点']
        },
        xAxis: {
            splitLine: {
                lineStyle: {
                    type: 'dashed'
                }
            }
        },
        yAxis: {
            splitLine: {
                lineStyle: {
                    type: 'dashed'
                }
            },
            scale: true
        },
        series: [{
            name: '参考点',
            type: 'scatter',
            data: dataList[3],
            itemStyle: {
                normal: {
                    shadowBlur: 10,
                    shadowColor: 'rgba(25, 100, 150, 0.5)',
                    shadowOffsetY: 5,
                    color: new echarts.graphic.RadialGradient(0.4, 0.3, 1, [{
                        offset: 0,
                        color: 'rgb(129, 227, 238)'
                    }, {
                        offset: 1,
                        color: 'rgb(25, 183, 207)'
                    }])
                }
            }
        },
        {
            name: '定位点',
            type: 'scatter',
            data: dataList[0],
            symbolSize: function (data) {
                return Math.sqrt(data[2]) * 1.5;
            },
            itemStyle: {
                normal: {
                    shadowBlur: 10,
                    shadowColor: 'rgba(120, 36, 50, 0.5)',
                    shadowOffsetY: 5,
                    color: new echarts.graphic.RadialGradient(0.4, 0.3, 1, [{
                        offset: 0,
                        color: 'rgb(251, 118, 123)'
                    }, {
                        offset: 1,
                        color: 'rgb(204, 46, 72)'
                    }])
                }
            }
        }
        ],
        backgroundColor: "#fff"
    };

    var option2 = {
        title: {
            text: '误差分布图'
        },
        tooltip: {},
        grid: {
            left: "10%",
            right: "10%",
            top: "10%",
            bottom: "10%"
        },
        legend: {
            data: ['误差值']
        },
        xAxis:[ {
            type: 'category',
            name:'定位点',
            data: dataList[1],
            position: 'bottom',
            axisLine: {
                onZero: false,
                lineStyle: {
                    color: '#668899'
                }
            },
        }],
        yAxis: {
            type: 'value',
            name:'误差值(cm)'
        },
        series: [{
            name: '误差值',
            type: 'line',
            data: dataList[2],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        }],
        backgroundColor: "#fff",
        animationDuration: 1000
    };
    // 使用刚指定的配置项和数据显示图表。
    myChart1.setOption(option1);
    myChart2.setOption(option2);
}