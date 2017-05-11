package queakviewer.smart.com.quakeviewer;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.Html;
import android.text.Spanned;
import android.widget.TextView;

import butterknife.BindView;
import butterknife.ButterKnife;

/**
 * Created by Ares on 2017/2/26.
 */

public class ResultActivity extends AppCompatActivity {

    private final static String format = "<p>&nbsp;&nbsp;&nbsp;&nbsp;尊敬的<strong><u>@UserName@</u></strong>先生/女士，您通过广东省地震局提供的“建筑房屋抗震能力即时简易评估系统”查询的房屋，" +
            "其在设防地震作用下为<strong><u>@DisplayMinorLevel@</u></strong>。" ;

    @BindView(R.id.description_result)
    TextView description;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_result);

        ButterKnife.bind(this);

        Intent intent =getIntent();
        Bundle bundle = intent.getBundleExtra("QuestionResult");
        String userName = bundle.getString("DisplayUserName");
        String displayMinorLevel = bundle.getString("DisplayMinorLevel");
        String displayReason1 = bundle.getString("DisplayReason1");
        String displayReason2 = bundle.getString("DisplayReason2");
        String displayReason3 = bundle.getString("DisplayReason3");

        String displayResult  = format.replace("@UserName@",userName)
                .replace("@DisplayMinorLevel@",displayMinorLevel)
                .replace("@DisplayReason1@",displayReason1)
                .replace("@DisplayReason2@",displayReason2)
                .replace("@DisplayReason3@",displayReason3);


        Spanned html = Html.fromHtml(displayResult);

        description.setText(html);
    }
}
