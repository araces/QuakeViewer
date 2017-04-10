package queakviewer.smart.com.quakeviewer;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.ScrollView;
import android.widget.Spinner;
import android.widget.SpinnerAdapter;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import queakviewer.smart.com.quakeviewer.Utils.FlowRadioGroup;
import queakviewer.smart.com.quakeviewer.Utils.LoadingDialog;
import queakviewer.smart.com.quakeviewer.Utils.MsgDialog;
import queakviewer.smart.com.quakeviewer.Utils.OnFinishedCallBack;
import queakviewer.smart.com.quakeviewer.Utils.Utils;
import queakviewer.smart.com.quakeviewer.Utils.WebClient;
import queakviewer.smart.com.quakeviewer.models.SelectItem;

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

    @BindView(R.id.iron_msg)
    Button ironMsg;
    @BindView(R.id.shuini_msg)
    Button shuiniMsg;

    @BindView(R.id.brike_msg)
    Button brikeMsg;

    @BindView(R.id.stone_msg)
    Button stoneMsg;

    @BindView(R.id.question_form)
    ScrollView scrollView;

    public List<SelectItem> areaList;

    public List<SelectItem> provinceList;
    public List<SelectItem> cityList;
    public List<SelectItem> regionList;

    private LoadingDialog loading;

    private Handler handler;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_question);

        ButterKnife.bind(this);


        buildLevel.setSelection(0);

        areaList = new ArrayList<SelectItem>();
        provinceList = new ArrayList<SelectItem>();
        cityList = new ArrayList<SelectItem>();
        regionList = new ArrayList<SelectItem>();

        structLevelGroup.check(R.id.structLevel);
        designedGroup.check(R.id.designed);
        jobstatusGroup.check(R.id.jobstatus);
        yearlevelGroup.check(R.id.yearlevel);

        scrollView.setHorizontalFadingEdgeEnabled(false);

        /*
        ironMsg.setPaintFlags(ironMsg.getPaintFlags() | Paint.UNDERLINE_TEXT_FLAG);
        shuiniMsg.setPaintFlags(shuiniMsg.getPaintFlags() | Paint.UNDERLINE_TEXT_FLAG);
        brikeMsg.setPaintFlags(brikeMsg.getPaintFlags() | Paint.UNDERLINE_TEXT_FLAG);
        stoneMsg.setPaintFlags(stoneMsg.getPaintFlags() | Paint.UNDERLINE_TEXT_FLAG);
        */
        loading = new LoadingDialog(this);

        handler = new Handler();

        new Thread(new Runnable() {
            @Override
            public void run() {
                QuestionActivity.this.queryAreas();
            }
        }).start();

        province.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                SelectItem item = provinceList.get(position);
                QuestionActivity.this.setCity(item.getKey());
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                return;
            }
        });

        city.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                SelectItem item = cityList.get(position);
                QuestionActivity.this.setRegion(item.getKey());
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        BuildLevelSpinnerAdapter levelsAdapter =new BuildLevelSpinnerAdapter(this.getApplicationContext());
        buildLevel.setAdapter(levelsAdapter);
        buildLevel.setSelection(0);
    }

    @OnClick({R.id.iron_msg, R.id.shuini_msg, R.id.brike_msg, R.id.stone_msg})
    public void showMsg(Button button) {

        if (button.getId() == R.id.iron_msg) {
            MsgDialog dialog = new MsgDialog(QuestionActivity.this, 0);
            dialog.show();
        }
        if (button.getId() == R.id.shuini_msg) {
            MsgDialog dialog = new MsgDialog(QuestionActivity.this, 1);
            dialog.show();
        }
        if (button.getId() == R.id.brike_msg) {
            MsgDialog dialog = new MsgDialog(QuestionActivity.this, 2);
            dialog.show();
        }
        if (button.getId() == R.id.stone_msg) {
            MsgDialog dialog = new MsgDialog(QuestionActivity.this, 3);
            dialog.show();
        }

    }


    @OnClick(R.id.question_query)
    public void submitQuery() {
        new Thread(new Runnable() {
            @Override
            public void run() {

                QuestionActivity.this.queryStructLevel();
            }
        }).start();
    }

    private void queryAreas() {
        loading.setMessage("加载区域信息");
        WebClient client = new WebClient();
        client.setOnDataArrivedListener(new OnFinishedCallBack() {
            @Override
            public void onDataArrived(String result) {
                JSONTokener jsonParser = new JSONTokener(result);
                try {
                    JSONObject jsonMain = (JSONObject) jsonParser.nextValue();
                    JSONObject jsonResult = jsonMain.getJSONObject("result");
                    if (jsonResult.getBoolean("success")) {

                        JSONArray jsonArray = jsonResult.getJSONArray("provinces");

                        for (int i = 0; i < jsonArray.length(); i++) {
                            JSONObject obj = jsonArray.getJSONObject(i);
                            SelectItem item = new SelectItem();
                            item.setKey(obj.getString("Id"));
                            item.setName(obj.getString("Name"));
                            item.setParentId(obj.getString("ParentId"));
                            areaList.add(item);
                        }

                        QuestionActivity.this.setProvince();

                    } else {

                        Toast.makeText(getApplicationContext(), "数据错误", Toast.LENGTH_LONG).show();
                    }
                } catch (JSONException ex) {
                    ex.printStackTrace();
                    Log.e(ID, ex.getMessage());
                    Toast.makeText(getApplicationContext(), "服务出错，请稍后再试", Toast.LENGTH_LONG).show();

                } finally {
                    if (loading.isShowing()) {
                        loading.dismiss();
                    }
                }
            }
        });

        try {
            String token = Utils.GetContent(getApplicationContext(), "token");
            client.GetData(StaticParams.AREAS_URL + token);
        } catch (IOException ex) {
            ex.printStackTrace();
            Log.e(ID, ex.getMessage());
            Toast.makeText(QuestionActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        loading.dismiss();
    }

    private void setProvince() {

        provinceList.clear();

        SelectItem item = new SelectItem();
        item.setKey("");
        item.setName("选择省");
        item.setKey("");
        provinceList.add(item);

        cityList.clear();

        SelectItem item2 = new SelectItem();
        item2.setKey("");
        item2.setName("选择市");
        item2.setKey("");
        cityList.add(item2);

        regionList.clear();

        SelectItem item3 = new SelectItem();
        item3.setKey("");
        item3.setName("选择区");
        item3.setKey("");
        regionList.add(item3);


        for (int i = 0; i < areaList.size(); i++) {
            if (areaList.get(i).getParentId().equals("0")) {
                provinceList.add(areaList.get(i));
            }
        }

        handler.post(new Runnable() {
            @Override
            public void run() {
                ExtendSpinnerAdapter provinceAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, provinceList);
                province.setAdapter(provinceAdapter);
                province.setSelection(0);

                ExtendSpinnerAdapter cityAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, cityList);
                city.setAdapter(cityAdapter);
                city.setSelection(0);

                ExtendSpinnerAdapter regionAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, regionList);
                region.setAdapter(regionAdapter);
                region.setSelection(0);
            }
        });
    }

    private void setCity(String provinceId) {

        cityList.clear();

        SelectItem item2 = new SelectItem();
        item2.setKey("");
        item2.setName("选择市");
        item2.setKey("");
        cityList.add(item2);

        regionList.clear();

        SelectItem item3 = new SelectItem();
        item3.setKey("");
        item3.setName("选择区");
        item3.setKey("");
        regionList.add(item3);

        for (int i = 0; i < areaList.size(); i++) {
            if (areaList.get(i).getParentId().equals(provinceId)) {
                cityList.add(areaList.get(i));
            }
        }
        handler.post(new Runnable() {
            @Override
            public void run() {
                ExtendSpinnerAdapter cityAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, cityList);
                city.setAdapter(cityAdapter);
                city.setSelection(0);

                ExtendSpinnerAdapter regionAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, regionList);
                region.setAdapter(regionAdapter);
                region.setSelection(0);
            }
        });
    }

    private void setRegion(String cityId) {
        regionList.clear();

        SelectItem item3 = new SelectItem();
        item3.setKey("");
        item3.setName("选择区");
        item3.setKey("");
        regionList.add(item3);

        for (int i = 0; i < areaList.size(); i++) {
            if (areaList.get(i).getParentId().equals(cityId)) {
                regionList.add(areaList.get(i));
            }
        }

        handler.post(new Runnable() {
            @Override
            public void run() {
                ExtendSpinnerAdapter regionAdapter = new ExtendSpinnerAdapter(QuestionActivity.this, regionList);
                region.setAdapter(regionAdapter);
                region.setSelection(0);
            }
        });

    }

    class ExtendSpinnerAdapter extends BaseAdapter implements SpinnerAdapter {
        private Context context;
        private List<SelectItem> items;

        public ExtendSpinnerAdapter(Context _context, List<SelectItem> _items) {
            this.context = _context;
            this.items = _items;
        }

        @Override
        public int getCount() {
            return items.size();
        }

        @Override
        public Object getItem(int position) {
            return this.items.get(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {

            convertView = LayoutInflater.from(context).inflate(R.layout.area_item, null);
            TextView tvgetView = (TextView) convertView.findViewById(R.id.dropdownitem);

            SelectItem item = items.get(position);

            tvgetView.setText(item.getName());
            return convertView;
        }
    }


    class BuildLevelSpinnerAdapter extends BaseAdapter implements SpinnerAdapter {
        private Context context;
        private List<String> items;

        public BuildLevelSpinnerAdapter(Context _context) {
            this.context = _context;
            items =new ArrayList<String>();
            items.add("选择楼层");
            for(Integer i=1;i<=200;i++){
                items.add(i+"层");
            }
        }

        @Override
        public int getCount() {
            return items.size();
        }

        @Override
        public Object getItem(int position) {
            return this.items.get(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {

            convertView = LayoutInflater.from(context).inflate(R.layout.area_item, null);
            TextView tvgetView = (TextView) convertView.findViewById(R.id.dropdownitem);

            String item = items.get(position);

            tvgetView.setText(item);
            return convertView;
        }
    }


    private void queryStructLevel() {


        String buildLevelSpinner = buildLevel.getSelectedItem().toString();

        RadioButton structLevelButton = (RadioButton) structLevelGroup.findViewById(structLevelGroup.getCheckedRadioButtonId());
        RadioButton designedButton = (RadioButton) designedGroup.findViewById(designedGroup.getCheckedRadioButtonId());
        RadioButton jobstatusButton = (RadioButton) jobstatusGroup.findViewById(jobstatusGroup.getCheckedRadioButtonId());
        RadioButton yearlevelButton = (RadioButton) yearlevelGroup.findViewById(yearlevelGroup.getCheckedRadioButtonId());

        if (buildLevelSpinner.equals("选择楼层")) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(QuestionActivity.this, "请选择楼层！", Toast.LENGTH_LONG).show();
                }
            });

            return;
        }

        String buildingLevelStr = buildLevelSpinner.replace("层", "");
        int builddingLevelInt = Integer.parseInt(buildingLevelStr);

        if (builddingLevelInt > 1 && structLevelButton.getTag().toString().equals("4")) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(QuestionActivity.this, "土石房屋总层数不能超过1层!", Toast.LENGTH_LONG).show();
                }
            });
            return;
        }

        if (builddingLevelInt > 10 && structLevelButton.getTag().toString().equals("3")) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(QuestionActivity.this, "砖砌房屋总层数不能超过10层！", Toast.LENGTH_LONG).show();
                }
            });
            return;
        }

        JSONObject param = new JSONObject();
        try {
            String token = Utils.GetContent(getApplicationContext(), "token");
            param.put("token", token);

            SelectItem item = (SelectItem) region.getSelectedItem();
            if (item.getKey().equals("")) {
                handler.post(new Runnable() {
                    @Override
                    public void run() {
                        Toast.makeText(QuestionActivity.this, "请选择区域", Toast.LENGTH_LONG).show();
                        region.requestFocus();
                    }
                });
                return;
            }

            param.put("regionId", item.getKey());
            param.put("storyNum", buildLevelSpinner.replace("层", ""));
            param.put("struType", structLevelButton.getTag());
            param.put("isDesigned", designedButton.getTag());
            param.put("contructionQuality", jobstatusButton.getTag());
            param.put("builtYearGroup", yearlevelButton.getTag());
        } catch (JSONException ex) {
            ex.printStackTrace();
            Log.e(ID, ex.getMessage());
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(QuestionActivity.this, "系统错误，请重试", Toast.LENGTH_LONG).show();
                }
            });

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

                        Bundle bundle = new Bundle();
                        bundle.putString("DisplayUserName", resultContent.getString("DisplayUserName"));
                        bundle.putString("DisplayMinorLevel", resultContent.getString("DisplayMinorLevel"));
                        bundle.putString("DisplayReason1", resultContent.getString("DisplayReason1"));
                        bundle.putString("DisplayReason2", resultContent.getString("DisplayReason2"));
                        bundle.putString("DisplayReason3", resultContent.getString("DisplayReason3"));


                        Intent intent = new Intent();
                        intent.putExtra("QuestionResult", bundle);
                        intent.setClass(QuestionActivity.this, ResultActivity.class);
                        QuestionActivity.this.startActivity(intent);
                    } else {
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                Toast.makeText(QuestionActivity.this, "数据错误", Toast.LENGTH_LONG).show();
                            }
                        });

                    }
                } catch (JSONException ex) {
                    ex.printStackTrace();
                    Log.e(ID, ex.getMessage());
                    handler.post(new Runnable() {
                        @Override
                        public void run() {
                            Toast.makeText(QuestionActivity.this, "服务出错，请稍后再试", Toast.LENGTH_LONG).show();
                        }
                    });


                } finally {
                    if (loading.isShowing()) {
                        loading.dismiss();
                    }
                }

            }
        });

        try {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    loading.show();
                }
            });

            client.PostData(StaticParams.QUESTION_URL, param.toString());
        } catch (IOException ex) {
            ex.printStackTrace();
            Log.e(ID, ex.getMessage());
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(QuestionActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
                }
            });

        }
    }
}
