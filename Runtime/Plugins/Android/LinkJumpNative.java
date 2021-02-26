package org.ftxtool.lib;

import android.content.Intent;
import android.net.Uri;

import com.unity3d.player.UnityPlayer;

public class LinkJumpNative {
    public static boolean CanOpenUrl(String url) {
        if (url == null)
            return false;

        if (url.isEmpty())
            return false;

        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        boolean canOpen = intent.resolveActivity(UnityPlayer.currentActivity.getPackageManager()) != null;

        return canOpen;
    }
}