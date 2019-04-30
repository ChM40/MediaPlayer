﻿using System;
using System.Threading.Tasks;
using System.Timers;

namespace ZPF.Media
{
   public enum MediaPlayerState
   {
      Playing,
      Paused,
      Stopped,
      Loading,
      Buffering,
      Failed
   }

   public interface IMediaPlayer
   {
      void Init();

      /// <summary>
      /// Reading the current status of the player
      /// </summary>
      MediaPlayerState State { get; }
      TimeSpan Position { get; }
      TimeSpan Duration { get; }

      TimeSpan StepSize { get; set; }

      bool IsInitialized { get; set; }

      IMediaExtractor MediaExtractor { get; set; }
      IVolumeManager VolumeManager { get; set; }


      /// <summary>
      /// Plays an uri (remote or local)
      /// </summary>
      /// <param name="uri"></param>
      /// <returns></returns>
      Task<IMediaItem> Play(string uri);

      /// <summary>
      /// Stops playing
      /// </summary>
      Task Stop();

      /// <summary>
      /// Changes position to the specified number of milliseconds from zero
      /// </summary>
      Task SeekTo(TimeSpan position);

      /// <summary>
      /// Seeks forward a fixed amount of seconds of the current MediaFile
      /// </summary>
      Task StepForward();

      /// <summary>
      /// Seeks backward a fixed amount of seconds of the current MediaFile
      /// </summary>
      Task StepBackward();

   }

   public abstract class MediaPlayerBase : IMediaPlayer
   {
      public Timer Timer { get; } = new Timer(1000);

      public MediaPlayerBase()
      {
         Timer.AutoReset = true;
         Timer.Elapsed += Timer_Elapsed;
         Timer.Start();
      }

      private TimeSpan PreviousPosition = new TimeSpan();
      protected virtual void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         if (!IsInitialized)
            return;

         //if (PreviousPosition != Position)
         //{
         //   PreviousPosition = Position;
         //   OnPositionChanged(this, new PositionChangedEventArgs(Position));
         //}
         //if (this.IsPlaying())
         //{
         //   OnPlayingChanged(this, new PlayingChangedEventArgs(Position, Duration));
         //}
         //if (this.IsBuffering())
         //{
         //   OnBufferingChanged(this, new BufferingChangedEventArgs(Buffered));
         //}
      }

      public TimeSpan StepSize { get; set; } = TimeSpan.FromSeconds(10);

      //ToDo: set private
      public bool IsInitialized { get; set; }

      public IMediaExtractor MediaExtractor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public IVolumeManager VolumeManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public abstract MediaPlayerState State { get; }
      public abstract TimeSpan Position { get; }
      public abstract TimeSpan Duration { get; }

      public abstract void Init();

      public abstract Task<IMediaItem> Play(string uri);

      public abstract Task Stop();



      // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  -

      public abstract Task SeekTo(TimeSpan position);

      public virtual Task StepBackward()
      {
         var seekTo = this.SeekTo(TimeSpan.FromSeconds(Double.IsNaN(Position.TotalSeconds) ? 0 : ((Position.TotalSeconds < StepSize.TotalSeconds) ? 0 : Position.TotalSeconds - StepSize.TotalSeconds)));
         Timer_Elapsed(null, null);
         return seekTo;
      }

      public virtual Task StepForward()
      {
         var seekTo = this.SeekTo(TimeSpan.FromSeconds(Double.IsNaN(Position.TotalSeconds) ? 0 : Position.TotalSeconds + StepSize.TotalSeconds));
         Timer_Elapsed(null, null);
         return seekTo;
      }

      // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  -
   }
}

