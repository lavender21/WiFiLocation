$.ajax({
    url: "/api/DataTest/",
    type:"GET",
    success: function (data) {
        if (data && data.hasOwnProperty('ds') && data.ds.length > 0) {
            addDataToSelect(data.ds);
        }
        else {
            $("select#memory").append('<option>无数据</option>');
            $('#check').attr('disabled', true);
        }
    },
    error: function () {
        $("select#memory").append('<option>无数据</option>');
        $('#check').attr('disabled', true);
        alert("未知错误");
    }
})

function addDataToSelect(data) {
    data.filter(function(item){
        return item.memory
    }).forEach(function (item) {
        $("select#memory").append('<option value='+item.id+'>'+item.memory+'</option>');
    });
}

$('#check').click(function () {
    $.ajax({
        url: "/api/DataTest/" + $("#memory").val(),
        tyep: "GET",
        success: function (data) {
            if (data != null) {
                generatePicture(data);
            }
            else {
                alert("无数据");
            }
        },
        error: function () {
            alert("未知错误");
        }
    })

});

function parseData(data) {
    var result = data.split(']');
    result.pop();
    return result.map(function (item) {
        var arr = item.split('[');
        return arr[1].split(',');
    });   
}

function generatePicture(data) {
    $('#coordShow').css('display', 'block');

    var coord = parseData(data.coord);

    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('coordShow'));

    // 指定图表的配置项和数据
    var option1 = {
        title: {
            text: '步行坐标展示'
        },
        tooltip: {},
        grid: {
            left: "10%",
            right: "10%",
            top: "10%",
            bottom: "10%",
            containLabel: true
        },
        legend: {
            data: ['步行坐标']
        },
        xAxis: {
            name: 'x(cm)',
            type:'value'
        },
        yAxis: {
            name: 'y(cm)',
            type:'value'
        },
        series: [{
            name: '步行坐标',
            type: 'line',
            smooth:true,
            data: coord
        }
        ],
        backgroundColor: "#fff"
    };
    console.log(option1)
    myChart1.setOption(option1);
}