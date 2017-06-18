//$.ajax({
//    url: "/api/Room/",
//    type: "GET",
//    success: function (data) {
//        if (data && data.hasOwnProperty('ds') && data.ds.length > 0) {
//            addDataToSelect(data.ds);
//        }
//        else {
//            $("select#memory").append('<option>无数据</option>');
//            $('#check').attr('disabled', true);
//        }
//    },
//    error: function () {
//        $("select#memory").append('<option>无数据</option>');
//        $('#check').attr('disabled', true);
//        alert("未知错误");
//    }
//});
var data = null;
var size = null;

function addDataToSelect(data) {
    data.filter(function (item) {
        return item.room_name
    }).forEach(function (item) {
        $("select#room").append('<option value=' + item.id + '>' + item.room_name + '</option>');
    });
}

$('#check').click(function () {
    $.ajax({
        url: "/api/Room?building=" + $("#build").val() + "&floor=" + $("#floor").val(),
        tyep: "GET",
        success: function (value) {
            if (value.ds != null) {
                data = value.ds;
                clearPicture();
                generateFloor();
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

$('#back').click(clearPicture);

var c = document.getElementById('myMap');
c.onclick = clickRoom;

function generateFloor() {
    $('#myMap').show();
    $('.tuli').show();
    size = caculateWidthHeight();
    c.width = parseInt(size.width);
    c.height = parseInt(size.height);
    var ctxt = c.getContext("2d");
    ctxt.fillStyle = "#fff";
    ctxt.fillRect(0, 0, c.width, c.height);
    
    ctxt.lineWidth = 2;
    ctxt.strokStyle = "#666";  

    data.forEach(function (item) {
        var left_down = item.left_down.split(',');
        var right_up = item.right_up.split(',');
        var width = Math.ceil(parseInt(right_up[0] - left_down[0])/size.multiple);
        var height = Math.ceil(parseInt(right_up[1] - left_down[1])/size.multiple);
        var coordX = Math.ceil(parseInt(left_down[0]) / size.multiple);
        var coordY = Math.ceil(parseInt(left_down[1]) / size.multiple);
     
        if (item.is_has_data) {
            ctxt.fillStyle = "#ff0";
        }
        else {
            ctxt.fillStyle = "#dde";
        }

        ctxt.strokeRect(coordX,coordY, width, height);
        ctxt.fillRect(coordX, coordY, width, height);
        ctxt.lineWidth = 1;
        ctxt.strokeStyle = "#000";
        ctxt.strokeText(item.room_name, coordX + width / 4, coordY + height / 2);
    });
}



// 选中拖动点
function clickRoom(e) {
    e = MousePos(e);  //获取鼠标位置
    var dx, dy;   //计算鼠标与拖动点圆心位置



    var hasDataRoom = getRoomHasData();
    hasDataRoom.forEach(function (item) {
        var coord1 = item.left_down.split(',');
        var coord2 = item.right_up.split(',');
        var x1 = Math.ceil(parseInt(coord1[0]) / size.multiple);
        var y1 = Math.ceil(parseInt(coord1[1]) / size.multiple);
        var x2 = Math.ceil(parseInt(coord2[0]) / size.multiple);
        var y2 = Math.ceil(parseInt(coord2[1]) / size.multiple);
        if (e.x > x1 && e.x < x2 && e.y > y1 && e.y < y2) {
            $.ajax({
                url: "/api/Room/" + item.id,
                tyep: "GET",
                success: function (value) {
                    if (value.ds != null) {
                        drawRoom(value);
                    }
                    else {
                        alert("无数据");
                    }
                },
                error: function () {
                    alert("未知错误");
                }
            });
        } else {
            alert("该房间未采集数据");
        }
    });

}

function clearPicture() {
    $('#myMap,.tuli').show();
    $('#myRoom,.count,#back').hide();
}

// 获取鼠标位置坐标
function MousePos(e) {
    var event = e || event;
    return {
        x: event.clientX - c.offsetLeft,
        y: event.clientY - c.offsetTop
    }
}

// 计算画布的宽高
function caculateWidthHeight() {
    var rightUpArray = data.map(function (item) {
        return item.right_up.split(',');
    });
    var widthArray = rightUpArray.map(function (item) {
        return item[0];
    });
    var heightArray = rightUpArray.map(function (item) {
        return item[1];
    });
    var maxWidth = Math.max.apply(null, widthArray);
    var maxHeight = Math.max.apply(null, heightArray);
    if (maxWidth > 1000) {
        multiple = maxWidth / 1000;
        maxHeight = Math.ceil(maxHeight / (multiple));
        maxWidth = 1000;
    }
    return { width: maxWidth,height: maxHeight,multiple:multiple || 1 };
}

//获取有数据的房间
function getRoomHasData() {
    return data.filter(function (item) {
        return item.is_has_data;
    });
}

//绘制房间
function drawRoom(value) {
    var room = value.ds[0];
    var log = value.ds1;
    var left_up = room.left_up.split(',');
    var right_down = room.right_down.split(',');
    var width = right_down[0] - left_up[0];
    var height = left_up[1] - right_down[1];
    $('#myMap,.tuli').hide();
    $('#myRoom,.count,#back').show();
    var dataList = log.map(function (item) {
        return item.location_coord.split(',');
    })
    var myChart1 = echarts.init(document.getElementById('myRoom'));

    var option1 = {
        title: {
            text: room.room_name
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
            max: width
        },
        yAxis: {
            name: 'y(cm)',
            min: 0,
            max: height
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
    $('#people').text(log.length);
}
