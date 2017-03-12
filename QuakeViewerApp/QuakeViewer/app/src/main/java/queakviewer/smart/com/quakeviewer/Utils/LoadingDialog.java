package queakviewer.smart.com.quakeviewer.Utils;

import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.widget.TextView;

import butterknife.BindView;
import butterknife.ButterKnife;
import queakviewer.smart.com.quakeviewer.R;

/**
 * Created by Ares on 2017/3/12.
 */

public class LoadingDialog extends Dialog {

    @BindView(R.id.tv_text)
     TextView tv_text;

    public LoadingDialog(Context context) {
        super(context);
        /**设置对话框背景透明*/
        getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        setContentView(R.layout.dialog_loading);
        ButterKnife.bind(this);
        setCanceledOnTouchOutside(false);
    }

    /**
     * 为加载进度个对话框设置不同的提示消息
     *
     * @param message 给用户展示的提示信息
     * @return build模式设计，可以链式调用
     */
    public LoadingDialog setMessage(String message) {
        tv_text.setText(message);
        return this;
    }
}
