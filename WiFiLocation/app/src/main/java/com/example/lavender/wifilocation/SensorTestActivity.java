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
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.util.Date;
import java.util.List;

/*这个页面用来做测试，通过步数和方向来记录轨迹计算坐标，采集数据存入数据库为后期做比对*/

public class SensorTestActivity extends AppCompatActivity implements View.OnClickListener{
    private Button startSensor, stopSensor;
    private TextView showSenserData;
    private EditText memory;
    private String txt = "";
    public static String res = "";

    // 接收服务传来的数据
    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            int[] coordThis = intent.getIntArrayExtra("coordThis");
            int[] coordPre = intent.getIntArrayExtra("coordPre");
            float degree = intent.getFloatExtra("degree",0);
            int xlen = intent.getIntExtra("xlen",0);
            int ylen = intent.getIntExtra("ylen",0);
            txt += "["+coordThis[0] + "," + coordThis[1]+"],";
            res = intent.getStringExtra("res");
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
        registerReceiver(receiver, new IntentFilter("httpResponse"));
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

    @Override
    protected void onDestroy() {
        unregisterReceiver(receiver);
        super.onDestroy();
    }

    // 初始化
    private void init() {
        startSensor = (Button) findViewById(R.id.btnStartSenser);
        stopSensor = (Button) findViewById(R.id.btnStopSenser);
        startSensor.setOnClickListener(this);
        stopSensor.setOnClickListener(this);
        showSenserData = (TextView) findViewById(R.id.showSenserData);
        memory = (EditText)findViewById(R.id.memory);
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
        // 将采集的数据发送到服务器
        JSONObject json = new JSONObject();

        try{
            json.put("coord",txt.substring(0,txt.length()-1));
            json.put("memory",memory.getText().toString());
            json.put("addtime",Common.getNowTime());
            json.put("flag",0);       // 0 测试坐标  1 测试wifi 2 测试坐标和wifi
        }catch (JSONException e)
        {
            e.printStackTrace();
        }
        HttpConnect httpConnect = new HttpConnect(new HttpConnect.AsyncResponse() {
            @Override
            public void processFinish(String output) {
                Toast.makeText(SensorTestActivity.this,output,Toast.LENGTH_SHORT).show();
            }
        });
        httpConnect.execute("POST", httpConnect.APIPOSTTEST,json.toString());
        Toast.makeText(SensorTestActivity.this,res,Toast.LENGTH_LONG).show();
    }

}
