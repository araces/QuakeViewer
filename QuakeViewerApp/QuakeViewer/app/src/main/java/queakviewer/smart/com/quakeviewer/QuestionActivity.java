package queakviewer.smart.com.quakeviewer;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import queakviewer.smart.com.quakeviewer.Utils.FlowRadioGroup;
import queakviewer.smart.com.quakeviewer.Utils.OnFinishedCallBack;
import queakviewer.smart.com.quakeviewer.Utils.Utils;
import queakviewer.smart.com.quakeviewer.Utils.WebClient;

/**
 * Created by Ares on 2017/2/26.
 */

public class QuestionActivity extends AppCompatActivity {

    public final static String ID = "QuestionActivity";

    @BindView(R.id.province)
    Spinner province;

    @BindView(R.id.city)
    Spinner city;

    @BindView(R.id.region)
    Spinner region;

    @BindView(R.id.buildLevel)
    Spinner buildLevel;

    @BindView(R.id.structLevel_question_group)
    FlowRadioGroup structLevelGroup;

    @BindView(R.id.designed_question_group)
    FlowRadioGroup designedGroup;

    @BindView(R.id.jobstatus_question_group)
    FlowRadioGroup jobstatusGroup;

    @BindView(R.id.yearlevel_question_group)
    FlowRadioGroup yearlevelGroup;

    @BindView(R.id.question_query)
    Button questionSubmit;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_question);

        ButterKnife.bind(this);


        buildLevel.setSelection(0);

        structLevelGroup.check(R.id.structLevel);
        designedGroup.check(R.id.designed);
        jobstatusGroup.check(R.id.jobstatus);
        yearlevelGroup.check(R.id.yearlevel);

        //questionSubmit.setOnClickListener(submitListener);

    }

    View.OnClickListener submitListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    QuestionActivity.this.queryStructLevel();
                }
            }).start();

        }

    };

    @OnClick(R.id.question_query)
    public void submitQuery(){
        new Thread(new Runnable() {
            @Override
            public void run() {
                QuestionActivity.this.queryStructLevel();
            }
        }).start();
    }


    private void queryStructLevel() {

        String buildLevelSpinner = buildLevel.getSelectedItem().toString();

        RadioButton structLevelButton = (RadioButton) structLevelGroup.findViewById(structLevelGroup.getCheckedRadioButtonId());
        RadioButton designedButton = (RadioButton) designedGroup.findViewById(designedGroup.getCheckedRadioButtonId());
        RadioButton jobstatusButton = (RadioButton) jobstatusGroup.findViewById(jobstatusGroup.getCheckedRadioButtonId());
        RadioButton yearlevelButton = (RadioButton) yearlevelGroup.findViewById(yearlevelGroup.getCheckedRadioButtonId());


        JSONObject param = new JSONObject();
        try {
            String token = Utils.GetContent(getApplicationContext(), "token");
            param.put("token", token);
            //param.put("region", region);
            param.put("region", "1");
            param.put("storyNum", buildLevelSpinner.replace("层",""));
            param.put("struType", structLevelButton.getTag());
            param.put("isDesigned", designedButton.getTag());
            param.put("contructionQuality", jobstatusButton.getTag());
            param.put("builtYearGroup", yearlevelButton.getTag());
        } catch (JSONException ex) {
            ex.printStackTrace();
            Log.e(ID, ex.getMessage());
            Toast.makeText(QuestionActivity.this, "系统错误，请重试", Toast.LENGTH_LONG);
        }


        WebClient client = new WebClient();
        client.setOnDataArrivedListener(new OnFinishedCallBack() {
            @Override
            public void onDataArrived(String result) {
                JSONTokener jsonParser = new JSONTokener(result);
                try {
                    JSONObject jsonMain = (JSONObject) jsonParser.nextValue();
                    JSONObject jsonResult = jsonMain.getJSONObject("result");
                    if (jsonResult.getBoolean("success")) {

                        JSONObject resultContent = jsonResult.getJSONObject("resultModel");

                        Bundle bundle =new Bundle();
                        bundle.putString("DisplayUserName",resultContent.getString("DisplayUserName"));
                        bundle.putString("DisplayMajorLevel",resultContent.getString("DisplayMajorLevel"));
                        bundle.putString("DisplayReason1",resultContent.getString("DisplayReason1"));
                        bundle.putString("DisplayReason2",resultContent.getString("DisplayReason2"));
                        bundle.putString("DisplayReason3",resultContent.getString("DisplayReason3"));


                        Intent intent = new Intent();
                        intent.putExtra("QuestionResult",bundle);
                        intent.setClass(QuestionActivity.this, ResultActivity.class);
                        QuestionActivity.this.startActivity(intent);
                    } else {

                        Toast.makeText(getApplicationContext(), "数据错误", Toast.LENGTH_LONG).show();
                    }
                } catch (JSONException ex) {
                    ex.printStackTrace();
                    Log.e(ID,ex.getMessage());
                    Toast.makeText(getApplicationContext(), "服务出错，请稍后再试", Toast.LENGTH_LONG).show();

                }

            }
        });

        try
        {
            client.PostData(StaticParams.QUESTION_URL,param.toString());
        }catch (IOException ex){
            ex.printStackTrace();
            Log.e(ID,ex.getMessage());
            Toast.makeText(QuestionActivity.this,"网络错误，请稍后重试",Toast.LENGTH_LONG).show();
        }
    }
}
