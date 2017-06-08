$.ajax({
    url: "/api/Room/",
    type: "GET",
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
    data.filter(function (item) {
        return item.room_name
    }).forEach(function (item) {
        $("select#room").append('<option value=' + item.id + '>' + item.room_name + '</option>');
    });
}

$('#check').click(function () {
    $.ajax({
        url: "/api/Room/" + $("#room").val(),
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

function generatePicture(data) {
    $('#myMap').show();
    $('.count').show();
    var room = data.ds[0];
    var log = data.ds1;
    var left_up = room.left_up.split(',');
    var right_down = room.right_down.split(',');
    var c = document.getElementById('myMap');
    c.width = parseInt(right_down[0]);
    c.height = parseInt(left_up[1]);
    var dataList = log.map(function (item) {
        return item.location_coord.split(',');
    })
    var myChart1 = echarts.init(document.getElementById('myMap'));

    var option1 = {
        title: {
            text: $('select#memory').val()
        },
        tooltip: {},
        grid: {
            left: "10%",
            right: "10%",
            top: "10%",
            bottom: "10%"
        },
        legend: {
            data: ['人员分布']
        },
        xAxis: {
            name: 'x(cm)',
            min: 0,
            max: c.width
        },
        yAxis: {
            name: 'y(cm)',
            min: 0,
            max: c.height
        },
        series: [{
            name: '人员分布',
            type: 'scatter',
            data: dataList
        }
        ],
        backgroundColor: "#fff",
        animationDuration: 1000
    };
    myChart1.setOption(option1);

    //var ctxt = c.getContext('2d');
    //ctxt.beginPath();
    //ctxt.moveTo(parseInt(left_up[0])+py, parseInt(left_up[1])+py);
    //ctxt.lineTo(parseInt(right_up[0])+py,parseInt(right_up[1])+py);
    //ctxt.lineTo(parseInt(right_down[0])+py, parseInt(right_down[1])+py);
    //ctxt.lineTo(parseInt(left_down[0]) + py, parseInt(left_down[1]) + py);
    //ctxt.lineTo(parseInt(left_up[0]) + py, parseInt(left_up[1]) + py);

    //ctxt.lineWidth = 5;
    //ctxt.strokStyle = "#666";
    //ctxt.fillStyle = "#ddddee";
    //ctxt.fill();
    //ctxt.stroke();

    //ctxt.lineStyle = "#f00";
    //ctxt.fillStyle = "#f00";
    //ctxt.lineWidth = 3;
    //for (var i = 0; i < log.length; i++) {
    //    var coord = log[i].location_coord.split(',');
    //    ctxt.beginPath();
    //    ctxt.arc(coord[0],coord[1], 5, 0, 2 * Math.PI);
    //    ctxt.fill();
    //    ctxt.stroke();
    //}
    $('#people').text(log.length);

}
