using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoServer.sdk;

namespace VideoServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MKSDK.on_mk_play_event mk_Play_Event = new MKSDK.on_mk_play_event(recvMKPlay_Event);
        MKSDK.on_mk_play_data mk_Play_Data = new MKSDK.on_mk_play_data(recvMKPlayData);
        public MainWindow()
        {
            InitializeComponent();

            init();
            test();
        }

        private void init()
        {
            MKSDK.mk_config mk_config = new MKSDK.mk_config
            {
                thread_num = 7,
                log_level = 0,
                log_file_path = @"log",
                log_file_days = 7,
                ini = @"config.ini",
                ini_is_path = 0,
                ssl = null,
                ssl_pwd = null
             
            };
            MKSDK.mk_env_init(ref mk_config);

            MKSDK.mk_http_server_start(80, 0);
            MKSDK.mk_http_server_start(443, 1);
            MKSDK.mk_rtsp_server_start(554, 0);
            MKSDK.mk_rtmp_server_start(1935, 0);
            MKSDK.mk_shell_server_start(9000);
            MKSDK.mk_rtp_server_start(10000);
        }

        private void test()
        {
            MKSDK.Context ctx = new MKSDK.Context();
            ctx.player = MKSDK.mk_player_create();
            MKSDK.mk_player_set_on_result(ctx.player, mk_Play_Event, ref ctx);
            MKSDK.mk_player_set_on_shutdown(ctx.player, mk_Play_Event, ref ctx);
            MKSDK.mk_player_set_on_data(ctx.player, mk_Play_Data, ref ctx);
            MKSDK.mk_player_play(ctx.player, "rtsp://10.20.30.176:554/PR0");
            ctx.push_url = "rtsp://127.0.0.1/live/test";
        }

        public static void recvMKPlay_Event(ref MKSDK.Context user_data, int err_code, [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string err_msg)
        {
            if(err_code == 0)//成功
            {

            }
        }

        public static void recvMKPlayData(ref MKSDK.Context user_data, int track_type, int codec_id, System.IntPtr data, int len, uint dts, uint pts)
        {

        }
    }
}
