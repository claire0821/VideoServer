using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VideoServer.sdk
{
    public class MKSDK
    {
        #region common
        #region 函数
        //初始化环境
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_env_init(ref mk_config cfg);

        /**
         * 关闭所有服务器，请在main函数退出时调用
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_stop_all_server();

        /**
         * 基础类型参数版本的mk_env_init，为了方便其他语言调用
         * @param thread_num 线程数
         * @param log_level 日志级别,支持0~4
         * @param log_file_path 文件日志保存路径,路径可以不存在(内部可以创建文件夹)，设置为NULL关闭日志输出至文件
         * @param log_file_days 文件日志保存天数,设置为0关闭日志文件
         * @param ini_is_path 配置文件是内容还是路径
         * @param ini 配置文件内容或路径，可以为NULL,如果该文件不存在，那么将导出默认配置至该文件
         * @param ssl_is_path ssl证书是内容还是路径
         * @param ssl ssl证书内容或路径，可以为NULL
         * @param ssl_pwd 证书密码，可以为NULL
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_env_init1(int thread_num,
                                              int log_level,
                                              ref string log_file_path,
                                              int log_file_days,
                                              int ini_is_path,
                                              ref string ini,
                                              int ssl_is_path,
                                              ref string ssl,
                                              ref string ssl_pwd);

        /**
         * 设置配置项
         * @param key 配置项名
         * @param val 配置项值
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_set_option(ref string key, ref string val);

        /**
         * 获取配置项的值
         * @param key 配置项名
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern string mk_get_option(ref string key);


        /**
         * 创建http[s]服务器
         * @param port htt监听端口，推荐80，传入0则随机分配
         * @param ssl 是否为ssl类型服务器
         * @return 0:失败,非0:端口号
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern UInt16 mk_http_server_start(UInt16 port, int ssl);

        /**
         * 创建rtsp[s]服务器
         * @param port rtsp监听端口，推荐554，传入0则随机分配
         * @param ssl 是否为ssl类型服务器
         * @return 0:失败,非0:端口号
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern UInt16 mk_rtsp_server_start(UInt16 port, int ssl);

        /**
         * 创建rtmp[s]服务器
         * @param port rtmp监听端口，推荐1935，传入0则随机分配
         * @param ssl 是否为ssl类型服务器
         * @return 0:失败,非0:端口号
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern UInt16 mk_rtmp_server_start(UInt16 port, int ssl);

        /**
         * 创建rtp服务器
         * @param port rtp监听端口(包括udp/tcp)
         * @return 0:失败,非0:端口号
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern UInt16 mk_rtp_server_start(UInt16 port);

        /**
         * 创建shell服务器
         * @param port shell监听端口
         * @return 0:失败,非0:端口号
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern UInt16 mk_shell_server_start(UInt16 port);
        #endregion

        #region 结构体
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct mk_config
        {
            // 线程数
            public int thread_num;

            // 日志级别,支持0~4
            public int log_level;
            //文件日志保存路径,路径可以不存在(内部可以创建文件夹)，设置为NULL关闭日志输出至文件
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
            public string log_file_path;
            //文件日志保存天数,设置为0关闭日志文件
            public int log_file_days;

            // 配置文件是内容还是路径
            public int ini_is_path;
            // 配置文件内容或路径，可以为NULL,如果该文件不存在，那么将导出默认配置至该文件
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
            public string ini;

            // ssl证书是内容还是路径
            public int ssl_is_path;
            // ssl证书内容或路径，可以为NULL
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
            public string ssl;
            // 证书密码，可以为NULL
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
            public string ssl_pwd;
        };
        #endregion
        #endregion

        #region player
        #region 函数
        /**
         * 播放结果或播放中断事件的回调
         * @param user_data 用户数据指针
         * @param err_code 错误代码，0为成功
         * @param err_msg 错误提示
         */
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public delegate void on_mk_play_event(IntPtr user_data, int err_code,string err_msg);
        public delegate void on_mk_play_event(ref Context user_data, int err_code, [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string err_msg);
        /**
         * 收到音视频数据回调
         * @param user_data 用户数据指针
         * @param track_type 0：视频，1：音频
         * @param codec_id 0：H264，1：H265，2：AAC 3.G711A 4.G711U 5.OPUS
         * @param data 数据指针
         * @param len 数据长度
         * @param dts 解码时间戳，单位毫秒
         * @param pts 显示时间戳，单位毫秒
         */
        //public static extern void on_mk_play_data (void* user_data, int track_type, int codec_id, void* data, size_t len, UInt32 dts, UInt32 pts);
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void on_mk_play_data(ref Context user_data, int track_type, int codec_id, System.IntPtr data, int len, uint dts, uint pts);
        /**
         * 创建一个播放器,支持rtmp[s]/rtsp[s]
         * @return 播放器指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr mk_player_create();

        /**
         * 销毁播放器
         * @param ctx 播放器指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_release(IntPtr ctx);

        /**
         * 设置播放器配置选项
         * @param ctx 播放器指针
         * @param key 配置项键,支持 net_adapter/rtp_type/rtsp_user/rtsp_pwd/protocol_timeout_ms/media_timeout_ms/beat_interval_ms/max_analysis_ms
         * @param val 配置项值,如果是整形，需要转换成统一转换成string
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_set_option(IntPtr ctx, string key, string val);

        /**
         * 开始播放url
         * @param ctx 播放器指针
         * @param url rtsp[s]/rtmp[s] url
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_play(IntPtr ctx, string url);

        /**
         * 暂停或恢复播放，仅对点播有用
         * @param ctx 播放器指针
         * @param pause 1:暂停播放，0：恢复播放
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_pause(IntPtr ctx, int pause);

        /**
         * 设置点播进度条
         * @param ctx 对象指针
         * @param progress 取值范围未 0.0～1.0
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_seekto(IntPtr ctx, float progress);

        /**
         * 设置播放器开启播放结果回调函数
         * @param ctx 播放器指针
         * @param cb 回调函数指针,设置null立即取消回调
         * @param user_data 用户数据指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_set_on_result(IntPtr ctx, on_mk_play_event cb, ref Context user_data);

        /**
         * 设置播放被异常中断的回调
         * @param ctx 播放器指针
         * @param cb 回调函数指针,设置null立即取消回调
         * @param user_data 用户数据指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_set_on_shutdown(IntPtr ctx, on_mk_play_event cb, ref Context user_data);

        /**
         * 设置音视频数据回调函数
         * @param ctx 播放器指针
         * @param cb 回调函数指针,设置null立即取消回调
         * @param user_data 用户数据指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_player_set_on_data(IntPtr ctx, on_mk_play_data cb, ref Context user_data);

        ///////////////////////////获取音视频相关信息接口在播放成功回调触发后才有效///////////////////////////////

        /**
         * 获取视频codec_id -1：不存在 0：H264，1：H265，2：AAC 3.G711A 4.G711U
         * @param ctx 播放器指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_video_codecId(IntPtr ctx);

        /**
         * 获取视频宽度
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_video_width(IntPtr ctx);

        /**
         * 获取视频高度
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_video_height(IntPtr ctx);

        /**
         * 获取视频帧率
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern float mk_player_video_fps(IntPtr ctx);

        /**
         * 获取音频codec_id -1：不存在 0：H264，1：H265，2：AAC 3.G711A 4.G711U
         * @param ctx 播放器指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_audio_codecId(IntPtr ctx);

        /**
         * 获取音频采样率
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_audio_samplerate(IntPtr ctx);

        /**
         * 获取音频采样位数，一般为16
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_audio_bit(IntPtr ctx);

        /**
         * 获取音频通道数
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_player_audio_channel(IntPtr ctx);

        /**
         * 获取点播节目时长，如果是直播返回0，否则返回秒数
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern float mk_player_duration(IntPtr ctx);

        /**
         * 获取点播播放进度，取值范围未 0.0～1.0
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern float mk_player_progress(IntPtr ctx);

        /**
         * 获取丢包率，rtsp时有效
         * @param ctx 对象指针
         * @param track_type 0：视频，1：音频
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern float mk_player_loss_rate(IntPtr ctx, int track_type);

        #endregion
        #region 结构体
        public struct Context
        {
            public IntPtr player;
            public IntPtr media;
            public IntPtr pusher;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string push_url;//推流地址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string pull_url;//拉流地址
        };
        #endregion
        #endregion

        #region media
        #region 函数
        /**
         * 创建一个媒体源
         * @param vhost 虚拟主机名，一般为__defaultVhost__
         * @param app 应用名，推荐为live
         * @param stream 流id，例如camera
         * @param duration 时长(单位秒)，直播则为0
         * @param hls_enabled 是否生成hls
         * @param mp4_enabled 是否生成mp4
         * @return 对象指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr mk_media_create([System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string vhost,
            [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string app,
            [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string stream,
                                             float duration, int hls_enabled, int mp4_enabled);

        /**
         * 销毁媒体源
         * @param ctx 对象指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_release(IntPtr ctx);

        /**
         * 添加视频轨道
         * @param ctx 对象指针
         * @param codec_id  0:CodecH264/1:CodecH265
         * @param width 视频宽度
         * @param height 视频高度
         * @param fps 视频fps
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_init_video(IntPtr ctx, int codec_id, int width, int height, float fps);

        /**
         * 添加音频轨道
         * @param ctx 对象指针
         * @param codec_id  2:CodecAAC/3:CodecG711A/4:CodecG711U/5:OPUS
         * @param channel 通道数
         * @param sample_bit 采样位数，只支持16
         * @param sample_rate 采样率
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_init_audio(IntPtr ctx, int codec_id, int sample_rate, int channels, int sample_bit);

        /**
         * 初始化h264/h265/aac完毕后调用此函数，
         * 在单track(只有音频或视频)时，因为ZLMediaKit不知道后续是否还要添加track，所以会多等待3秒钟
         * 如果产生的流是单Track类型，请调用此函数以便加快流生成速度，当然不调用该函数，影响也不大(会多等待3秒)
         * @param ctx 对象指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_init_complete(IntPtr ctx);

        /**
         * 输入单帧H264视频，帧起始字节00 00 01,00 00 00 01均可
         * @param ctx 对象指针
         * @param data 单帧H264数据
         * @param len 单帧H264数据字节数
         * @param dts 解码时间戳，单位毫秒
         * @param pts 播放时间戳，单位毫秒
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_input_h264(IntPtr ctx, IntPtr data, int len, UInt32 dts, UInt32 pts);

        /**
         * 输入单帧H265视频，帧起始字节00 00 01,00 00 00 01均可
         * @param ctx 对象指针
         * @param data 单帧H265数据
         * @param len 单帧H265数据字节数
         * @param dts 解码时间戳，单位毫秒
         * @param pts 播放时间戳，单位毫秒
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_input_h265(IntPtr ctx, IntPtr data, int len, UInt32 dts, UInt32 pts);

        /**
         * 输入单帧AAC音频(单独指定adts头)
         * @param ctx 对象指针
         * @param data 不包含adts头的单帧AAC数据
         * @param len 单帧AAC数据字节数
         * @param dts 时间戳，毫秒
         * @param adts adts头，可以为null
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_input_aac(IntPtr ctx, IntPtr data, int len, UInt32 dts, IntPtr adts);

        /**
         * 输入单帧PCM音频,启用ENABLE_FAAC编译时，该函数才有效
         * @param ctx 对象指针
         * @param data 单帧PCM数据
         * @param len 单帧PCM数据字节数
         * @param dts 时间戳，毫秒
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_input_pcm(IntPtr ctx, IntPtr data, int len, UInt32 pts);

        /**
         * 输入单帧OPUS/G711音频帧
         * @param ctx 对象指针
         * @param data 单帧音频数据
         * @param len  单帧音频数据字节数
         * @param dts 时间戳，毫秒
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_input_audio(IntPtr ctx, IntPtr data, int len, UInt32 dts);

        /**
         * MediaSource.close()回调事件
         * 在选择关闭一个关联的MediaSource时，将会最终触发到该回调
         * 你应该通过该事件调用mk_media_release函数并且释放其他资源
         * 如果你不调用mk_media_release函数，那么MediaSource.close()操作将无效
         * @param user_data 用户数据指针，通过mk_media_set_on_close函数设置
         */
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void on_mk_media_close (IntPtr user_data);

        /**
         * 监听MediaSource.close()事件
         * 在选择关闭一个关联的MediaSource时，将会最终触发到该回调
         * 你应该通过该事件调用mk_media_release函数并且释放其他资源
         * @param ctx 对象指针
         * @param cb 回调指针
         * @param user_data 用户数据指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_set_on_close(IntPtr ctx, on_mk_media_close cb, IntPtr user_data);

        /**
         * 收到客户端的seek请求时触发该回调
         * @param user_data 用户数据指针,通过mk_media_set_on_seek设置
         * @param stamp_ms seek至的时间轴位置，单位毫秒
         * @return 1代表将处理seek请求，0代表忽略该请求
         */
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate int on_mk_media_seek (IntPtr user_data, UInt32 stamp_ms);

        /**
         * 监听播放器seek请求事件
         * @param ctx 对象指针
         * @param cb 回调指针
         * @param user_data 用户数据指针
         */
         [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_set_on_seek(IntPtr ctx, on_mk_media_seek cb, IntPtr user_data);

        /**
         * 获取总的观看人数
         * @param ctx 对象指针
         * @return 观看人数
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_media_total_reader_count(IntPtr ctx);

        /**
         * 生成的MediaSource注册或注销事件
         * @param user_data 设置回调时的用户数据指针
         * @param sender 生成的MediaSource对象
         * @param regist 1为注册事件，0为注销事件
         */
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void on_mk_media_source_regist (IntPtr user_data, IntPtr sender, int regist);

        /**
         * 设置MediaSource注册或注销事件回调函数
         * @param ctx 对象指针
         * @param cb 回调指针
         * @param user_data 用户数据指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_set_on_regist(IntPtr ctx, on_mk_media_source_regist cb, IntPtr user_data);

        /**
         * rtp推流成功与否的回调(第一次成功后，后面将一直重试)
         */
        //[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //typedef on_mk_media_source_send_rtp_result on_mk_media_send_rtp_result;
        /**
         * rtp推流成功与否的回调(第一次成功后，后面将一直重试)
         */
        public delegate void on_mk_media_source_send_rtp_result (IntPtr user_data, UInt16 local_port, int err,
            [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string msg);
        /**
         * 开始发送ps-rtp流
         * @param ctx 对象指针
         * @param dst_url 目标ip或域名
         * @param dst_port 目标端口
         * @param ssrc rtp的ssrc，10进制的字符串打印
         * @param is_udp 是否为udp
         * @param cb 启动成功或失败回调
         * @param user_data 回调用户指针
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern void mk_media_start_send_rtp(IntPtr ctx,
            [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string dst_url, 
            UInt16 dst_port, [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string ssrc,
            int is_udp, on_mk_media_source_send_rtp_result cb, IntPtr user_data);

        /**
         * 停止ps-rtp发送
         * @param ctx 对象指针
         * @return 1成功，0失败
         */
        [DllImport("mk_api.dll", CharSet = CharSet.Ansi)]
        public static extern int mk_media_stop_send_rtp(ref Context ctx);
        #endregion

        #region 结构体
        #endregion
        #endregion
    }
}
