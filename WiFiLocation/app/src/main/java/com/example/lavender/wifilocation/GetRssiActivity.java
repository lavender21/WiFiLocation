package com.example.lavender.wifilocation;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.test.suitebuilder.TestMethod;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;
import org.w3c.dom.Text;

import static com.example.lavender.wifilocation.Common.toJson;

public class GetRssiActivity extends AppCompatActivity implements View.OnClickListener {
    private Button btnStartGetRssi,btnStopGetRssi;
    private EditText edtCoord,edtMemory;
    public static TextView txtShow;
    private String apData = "";
    public static String httpRes = "";
    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            apData = intent.getStringExtra("ap");
            txtShow.setText(apData);
        }
    };
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_get_rssi);
        // 初始化
        init();

        // 注册监听事件
        registerReceiver(receiver,new IntentFilter("apData"));
    }

    @Override
    public void onClick(View v) {
        switch (v.getId())
        {
            case R.id.btnStartGetRssi:
                startScan();
                break;
            case R.id.btnStopGetRssi:
                stopScan();
                break;
        }
    }

    // 初始化
    private void init()
    {
        btnStartGetRssi = (Button)findViewById(R.id.btnStartGetRssi);
        btnStartGetRssi.setOnClickListener(this);
        btnStopGetRssi = (Button)findViewById(R.id.btnStopGetRssi);
        btnStopGetRssi.setOnClickListener(this);
        edtCoord = (EditText)findViewById(R.id.edtCoord);
        edtMemory = (EditText)findViewById(R.id.edtMemory);
        txtShow = (TextView)findViewById(R.id.txtShow);
    }

    // 开始扫描wifi信号强度
    public void startScan()
    {
        Intent intent = new Intent(GetRssiActivity.this,GetRSSIService.class);
        startService(intent);
    }

    // 结束扫描wifi信号强度
    public void stopScan()
    {
        Intent intent = new Intent(GetRssiActivity.this,GetRSSIService.class);
        stopService(intent);
        sendDataToServer();
    }

    // 发送数据到服务器
    public void sendDataToServer()
    {
        JSONObject json = new JSONObject();
     /*   Gson gson = new Gson();
        JSONObject apJson = gson.fromJson(apData,JSONObject.class);*/
        try{
            json.put("ap",toJson(apData));
            json.put("coord",edtCoord.getText().toString());
            json.put("memory",edtMemory.getText().toString());
            json.put("flag",0);
            json.put("addtime",Common.getNowTime());
            json.put("mobile_id",Common.getMobileModel());
        }catch (JSONException e)
        {
            e.printStackTrace();
        }
        HttpConnect httpConnect = new HttpConnect();
/*
        String data = gson.toJson(json);
*/
        httpConnect.execute("POST",httpConnect.FINGERPRINT,json.toString());
//        Toast.makeText(this,httpRes,Toast.LENGTH_LONG).show();
    }
}
