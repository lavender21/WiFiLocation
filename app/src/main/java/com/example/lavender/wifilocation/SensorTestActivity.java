package com.example.lavender.wifilocation;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.util.List;

/*这个页面用来做测试，通过步数和方向来记录轨迹计算坐标，采集数据存入数据库为后期做比对*/

public class SensorTestActivity extends AppCompatActivity implements View.OnClickListener{
    private Button startSensor, stopSensor;
    private TextView showSenserData;
    private Sensor accSensor;
    private Sensor oriSensor;

    private String txt = "当前位置(cm)：";
    // 接收服务传来的数据
    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            int[] coordThis = intent.getIntArrayExtra("coordThis");
            int[] coordPre = intent.getIntArrayExtra("coordPre");
            float degree = intent.getFloatExtra("degree",0);
            int xlen = intent.getIntExtra("xlen",0);
            int ylen = intent.getIntExtra("ylen",0);
            txt += "coordThis:["+coordThis[0] + "," + coordThis[1] + "," + coordThis[2]+
                    "]coordPre:["+coordPre[0] + "," + coordPre[1] + "," + coordPre[2]+
                    "xlen:"+xlen+",ylen:"+ylen+",degree:"+degree+"\n";
            showSenserData.setText(txt);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sensor_test);
        init();

        //注册添加对象
        registerReceiver(receiver, new IntentFilter("sensorData"));
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            // 开始采集
            case R.id.btnStartSenser:
                startGetCoordData();
                break;
            // 结束采集
            case R.id.btnStopSenser:
                stopGetCoordData();
                break;
        }
    }

    // 初始化
    private void init() {
        startSensor = (Button) findViewById(R.id.btnStartSenser);
        stopSensor = (Button) findViewById(R.id.btnStopSenser);
        startSensor.setOnClickListener(this);
        stopSensor.setOnClickListener(this);
        showSenserData = (TextView) findViewById(R.id.showSenserData);
    }

    // 开始采集坐标数据
    private void startGetCoordData() {
        Intent intent = new Intent(SensorTestActivity.this, GetCoordService.class);
        startService(intent);
    }

    //  结束采集坐标数据
    private void stopGetCoordData() {
        Intent intent = new Intent(SensorTestActivity.this, GetCoordService.class);
        stopService(intent);
    }

}
