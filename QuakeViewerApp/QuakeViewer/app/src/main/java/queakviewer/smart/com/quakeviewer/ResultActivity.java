package queakviewer.smart.com.quakeviewer;

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

    private final static String format = "<html><head></head><body><p style=\"font-size: 20;\">      尊敬的<strong><u>@Html.Raw(Model.UserName)</u></strong>先生/女士，您通过广东省地震局提供的“建筑房屋抗震能力即时评估系统”查询的房屋，其在设防地震作用下为<strong><u>@Html.Raw(Model.DisplayMajorLevel)</u></strong>。根据您输入的房屋参数，其可能的原因为<strong><u id=\"result\">@Html.Raw(Model.DisplayReason1)，@Html.Raw(Model.DisplayReason2)，@Html.Raw(Model.DisplayReason3)</u></strong>。</p></body></html>";

    @BindView(R.id.description_result)
    TextView description;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        setContentView(R.layout.activity_result);

        ButterKnife.bind(this);

        Spanned html = Html.fromHtml(format);

        description.setText(html);
    }
}
