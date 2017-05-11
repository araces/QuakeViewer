package queakviewer.smart.com.quakeviewer;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import okhttp3.OkHttpClient;
import queakviewer.smart.com.quakeviewer.Utils.LoadingDialog;
import queakviewer.smart.com.quakeviewer.Utils.OnFinishedCallBack;
import queakviewer.smart.com.quakeviewer.Utils.Utils;
import queakviewer.smart.com.quakeviewer.Utils.WebClient;

/**
 * Created by Ares on 2017/2/25.
 */

public class RegistActivity extends AppCompatActivity {

    private final static String ID = "RegistActivity";

    @BindView(R.id.regist_userName)
    EditText mUserName;

    @BindView(R.id.regist_password)
    EditText mPassword;

    @BindView(R.id.regist_confirmpassword)
    EditText mConfirmPassword;

    Handler handler;

    LoadingDialog loading;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_regist);

        ButterKnife.bind(this);
        handler = new Handler();
        loading =new LoadingDialog(this);
    }

    @OnClick(R.id.go_login)
    public void goLogin() {
        Intent intent = new Intent();
        intent.setClass(RegistActivity.this, LoginActivity.class);
        startActivity(intent);
        finish();
    }

    @OnClick(R.id.regist_submit)
    public void submit(Button button) {

        new Thread(new Runnable() {
            @Override
            public void run() {
                RegistActivity.this.RegistUser();
            }
        }).start();


    }

    private void RegistUser() {
        if (!checkUserName(mUserName)) {
            return;
        }
        if (!checkPassword(mPassword, mConfirmPassword)) {
            return;
        }


        JSONObject param = new JSONObject();
        try {
            param.put("userName", mUserName.getText());
            param.put("password", mPassword.getText());
            //param.put("mobile", mEmail.getText());
        } catch (JSONException ex) {

            Toast.makeText(RegistActivity.this, "系统错误，请重试", Toast.LENGTH_LONG).show();
        }

        WebClient client = new WebClient();
        client.setOnDataArrivedListener(new OnFinishedCallBack() {
            @Override
            public void onDataArrived(String result) {
                JSONTokener jsonParser = new JSONTokener(result);
                try {
                    JSONObject jsonResult = (JSONObject) jsonParser.nextValue();
                    JSONObject resultContent = (JSONObject) jsonResult.getJSONObject("result");
                    if (resultContent.getBoolean("success")) {
                        //String token = resultContent.getString("token");
                        //Utils.SaveContent(getApplicationContext(), "token", token);

                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                Toast.makeText(RegistActivity.this, "注册成功", Toast.LENGTH_LONG).show();
                            }
                        });

                        Intent intent = new Intent();
                        intent.setClass(RegistActivity.this,LoginActivity.class);
                        RegistActivity.this.startActivity(intent);
                        finish();

                    }
                    else{
                        final String msg = resultContent.getString("msg");
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                Toast.makeText(RegistActivity.this, msg, Toast.LENGTH_LONG).show();
                            }
                        });
                    }
                } catch (Exception ex) {

                    handler.post(new Runnable() {
                        @Override
                        public void run() {
                            Toast.makeText(RegistActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
                        }
                    });

                }
                finally {
                    if(loading.isShowing()){
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
            client.PostData(StaticParams.REGIST_URL, param.toString());
        } catch (IOException ex) {

            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(RegistActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
                }
            });

        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        loading.dismiss();
    }

    private boolean checkUserName(final EditText userName) {
        if (TextUtils.isEmpty(userName.getText())) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    userName.setError("用户名不能为空");
                    userName.requestFocus();
                }
            });

            return false;
        }

        return true;
    }

    private boolean checkPassword(final EditText password,final EditText confirmPassword) {
        if (TextUtils.isEmpty(password.getText())) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    password.setError("密码不能为空");
                    password.requestFocus();
                }
            });

            return false;
        }
        if (password.getText().length() < 3) {
           handler.post(new Runnable() {
               @Override
               public void run() {
                   password.setError("密码不能小于3位");
                   password.requestFocus();
               }
           });
            return false;
        }

        if (TextUtils.isEmpty(confirmPassword.getText())) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    confirmPassword.setError("确认密码不能为空");
                    confirmPassword.requestFocus();
                }
            });

            return false;
        }


        if (!confirmPassword.getText().toString().equals(password.getText().toString())) {
            handler.post(new Runnable() {
                @Override
                public void run() {
                    confirmPassword.setError("两次输入密码不一致");
                    confirmPassword.requestFocus();
                }
            });

            return false;
        }

        return true;
    }

    private boolean checkMobile(EditText mobile) {
        Pattern pattern = Pattern.compile("^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$");
        // Pattern pat = Pattern.compile(regEx, Pattern.CASE_INSENSITIVE);
        Matcher matcher = pattern.matcher(mobile.getText());
        // 字符串是否与正则表达式相匹配
        boolean isMatch = matcher.matches();
        if (!isMatch) {
            if (!mobile.getText().equals(mobile.getText())) {
                mobile.setError("手机号码格式错误");
                mobile.requestFocus();
                return false;
            }
        }

        return true;
    }

    private boolean checkEmail(EditText email) {
        Pattern pattern = Pattern.compile("/^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\\.][a-z]{2,3}([\\.][a-z]{2})?$/i");
        // Pattern pat = Pattern.compile(regEx, Pattern.CASE_INSENSITIVE);
        Matcher matcher = pattern.matcher(email.getText());
        // 字符串是否与正则表达式相匹配
        boolean isMatch = matcher.matches();
        if (!isMatch) {
            if (!email.getText().equals(email.getText())) {
                email.setError("邮箱码格式错误");
                email.requestFocus();
                return false;
            }
        }

        return true;
    }


}
