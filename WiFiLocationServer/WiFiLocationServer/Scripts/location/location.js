$('#locationShow').click(function () {
    var data = getSelectData();
    $.ajax({
        url: "/api/LocationLog/?room=" + data.room + "&mobile=" + data.mobile + "&algorithm=" + data.algorithm,
        success: function (data) {
            if (data.hasOwnProperty('ds') && data.ds.length > 0 ){
                generatePicture(data.ds);
            }
            else {
                alert("返回数据有误！");
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
   result[0] =  data.map(function (item) {
       return item.actual_coord.split(',');
   });
    result[1] = data.map(function (item) {
        return item.location_coord.split(',');
    });
    result[2] = result[0].map(function (item,index) {
        var location = result[1][index];
        return Math.sqrt((item[0] - location[0])*(item[0] - location[0]) + (item[1] - location[1])*(item[1] - location[1])).toFixed(2);
    });
    return result;
}

function generatePicture(data) {
    // 组织数据
    var dataList = convertData(data);

    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('position'));
    var myChart2 = echarts.init(document.getElementById('deviation'));

    // 指定图表的配置项和数据
    var option1 = {
        title: {
            text: '定位坐标与实际坐标分布图'
        },
        tooltip: {},
        grid:{
            left: "10%",
            right: "10%",
            top: "10%",
            bottom:"10%"
        },
        legend: {
            data: ['定位值','实际值']
        },
        xAxis: {
            name:'x(cm)',
            min: 0,
            max:240
        },
        yAxis: {
            name:'y(cm)',
            min: 0,
            max:720
        },
        series: [{
            name: '定位值',
            type: 'scatter',
            data: dataList[1]
        },
        {
            name: '实际值',
            type: 'scatter',
            data: dataList[0]
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
            name:'定位坐标',
            data: dataList[1],
            position: 'bottom',
            axisLine: {
                onZero: false,
                lineStyle: {
                    color: '#668899'
                }
            },
        }, {
            type: 'category',
            name: '实际坐标',
            data: dataList[0],
            position: 'top',
            axisLine: {
                onZero: false,
                lineStyle: {
                    color: '#ff7575'
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