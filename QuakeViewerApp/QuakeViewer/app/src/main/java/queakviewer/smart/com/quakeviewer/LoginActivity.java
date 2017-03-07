package queakviewer.smart.com.quakeviewer;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Handler;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;

import android.os.AsyncTask;

import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.inputmethod.EditorInfo;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import queakviewer.smart.com.quakeviewer.Utils.OnFinishedCallBack;
import queakviewer.smart.com.quakeviewer.Utils.Utils;
import queakviewer.smart.com.quakeviewer.Utils.WebClient;

/**
 * A login screen that offers login via email/password.
 */
public class LoginActivity extends AppCompatActivity {

    /**
     * Id to identity READ_CONTACTS permission request.
     */
    private static final int REQUEST_READ_CONTACTS = 0;


    private static final String ID = "LoginActivity";


    /**
     * A dummy authentication store containing known user names and passwords.
     * TODO: remove after connecting to a real authentication system.
     */
    private static final String[] DUMMY_CREDENTIALS = new String[]{
            "foo@example.com:hello", "bar@example.com:world"
    };

    Handler handler =new Handler();


    // UI references.
    @BindView(R.id.userName) EditText mUserNameView;
    @BindView(R.id.password) EditText mPasswordView;
    @BindView(R.id.login_progress) View mProgressView;
    @BindView(R.id.login_form) View mLoginFormView;
    @BindView(R.id.login_signin) Button mSignInButton;
    @BindView(R.id.go_register) Button mGoRegistView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_login);

        ButterKnife.bind(this);

        //populateAutoComplete();

        mPasswordView.setOnEditorActionListener(new TextView.OnEditorActionListener() {
            @Override
            public boolean onEditorAction(TextView textView, int id, KeyEvent keyEvent) {
                if (id == R.id.login || id == EditorInfo.IME_NULL) {
                    attemptLogin();
                    return true;
                }
                return false;
            }
        });


        mSignInButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View view) {
                attemptLogin();
            }
        });

        mLoginFormView = findViewById(R.id.login_form);
        mProgressView = findViewById(R.id.login_progress);

        InputMethodManager imm =  (InputMethodManager)getSystemService(Context.INPUT_METHOD_SERVICE);
        if(imm !=null){
            imm.hideSoftInputFromWindow(LoginActivity.this.getWindow().getDecorView().getWindowToken(),0);
        }
    }

/*
    private void populateAutoComplete() {
        if (!mayRequestContacts()) {
            return;
        }

        getLoaderManager().initLoader(0, null, this);
    }

    private boolean mayRequestContacts() {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M) {
            return true;
        }
        if (checkSelfPermission(READ_CONTACTS) == PackageManager.PERMISSION_GRANTED) {
            return true;
        }
        if (shouldShowRequestPermissionRationale(READ_CONTACTS)) {
            Snackbar.make(mEmailView, R.string.permission_rationale, Snackbar.LENGTH_INDEFINITE)
                    .setAction(android.R.string.ok, new View.OnClickListener() {
                        @Override
                        @TargetApi(Build.VERSION_CODES.M)
                        public void onClick(View v) {
                            requestPermissions(new String[]{READ_CONTACTS}, REQUEST_READ_CONTACTS);
                        }
                    });
        } else {
            requestPermissions(new String[]{READ_CONTACTS}, REQUEST_READ_CONTACTS);
        }
        return false;
    }


*/

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        if (requestCode == REQUEST_READ_CONTACTS) {
            if (grantResults.length == 1 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {

            }
        }
    }


    /**
     * Attempts to sign in or register the account specified by the login form.
     * If there are form errors (invalid email, missing fields, etc.), the
     * errors are presented and no actual login attempt is made.
     */
    private void attemptLogin() {


        // Reset errors.
        mUserNameView.setError(null);
        mPasswordView.setError(null);

        // Store values at the time of the login attempt.
        final String userName = mUserNameView.getText().toString();
        final String password = mPasswordView.getText().toString();

        boolean cancel = false;
        View focusView = null;

        // Check for a valid password, if the user entered one.
        if (!TextUtils.isEmpty(password) && !isPasswordValid(password)) {
            mPasswordView.setError(getString(R.string.error_invalid_password));
            focusView = mPasswordView;
            cancel = true;
        }

        // Check for a valid email address.
        if (TextUtils.isEmpty(userName)) {
            mUserNameView.setError(getString(R.string.error_field_required));
            focusView = mUserNameView;
            cancel = true;
        } else if (!isUserNameValid(userName)) {
            mUserNameView.setError(getString(R.string.error_invalid_email));
            focusView = mUserNameView;
            cancel = true;
        }

        if (cancel) {
            // There was an error; don't attempt login and focus the first
            // form field with an error.
            focusView.requestFocus();
        } else {
            // Show a progress spinner, and kick off a background task to
            // perform the user login attempt.

            //mAuthTask = new UserLoginTask(userName, password);
            //mAuthTask.execute((Void) null);
            new Thread(new Runnable() {
                @Override
                public void run() {
                    LoginActivity.this.LoginIn(userName,password);
                }
            }).start();

        }
    }

    private void LoginIn(String userName,String password){


            WebClient client =new WebClient();
            client.setOnDataArrivedListener(new OnFinishedCallBack() {
                @Override
                public void onDataArrived(String result)  {
                    JSONTokener jsonParser = new JSONTokener(result);
                    try {
                        JSONObject jsonResult = (JSONObject) jsonParser.nextValue();
                        JSONObject resultContent = (JSONObject) jsonResult.getJSONObject("result");
                        if(resultContent.getBoolean("success")){
                            String token = resultContent.getString("token");
                            Utils.SaveContent(getApplicationContext(),"token",token);
                            Intent intent=new Intent();
                            intent.setClass(LoginActivity.this,QuestionActivity.class);
                            LoginActivity.this.startActivity(intent);
                            LoginActivity.this.finish();
                        }
                        else{
                            final String msg =  resultContent.getString("msg");
                            handler.post(new Runnable() {
                                @Override
                                public void run() {
                                    Toast.makeText(getApplicationContext(),msg,Toast.LENGTH_LONG).show();
                                }
                            });

                        }
                    }catch (JSONException e){
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                Toast.makeText(getApplicationContext(),"服务出错，请稍后再试",Toast.LENGTH_LONG).show();
                            }
                        });


                    }

                }
            });


            JSONObject param = new JSONObject();
            try {
                param.put("userName", userName);
                param.put("password", password);
            }
            catch (JSONException ex) {
                ex.printStackTrace();
                Log.e(ID, ex.getMessage());
                handler.post(new Runnable() {
                    @Override
                    public void run() {
                        Toast.makeText(LoginActivity.this, "系统错误，请重试", Toast.LENGTH_LONG);
                    }
                });
            }

            try
            {
                client.PostData(StaticParams.LOGIN_URL,param.toString());
            }catch (IOException ex) {
                ex.printStackTrace();
                Log.e(ID, ex.getMessage());
                handler.post(new Runnable() {
                    @Override
                    public void run() {
                        Toast.makeText(LoginActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
                    }
                });
            }


    }

    private boolean isUserNameValid(String userName) {
        //TODO: Replace this with your own logic
        return true;
    }

    private boolean isPasswordValid(String password) {
        //TODO: Replace this with your own logic
        return password.length() >= 6;
    }




    @OnClick(R.id.go_register)
    public void goRegist(){
        Intent intent=new Intent();
        intent.setClass(LoginActivity.this,RegistActivity.class);
        startActivity(intent);

    }

}

