﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZPF.Media
{
   public class MediaPlayer : MediaPlayerBase
   {
      // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  -

      static MediaPlayer _Current = null;

      public static MediaPlayer Current
      {
         get
         {
            if (_Current == null)
            {
               _Current = new MediaPlayer();
            };

            return _Current;
         }

         set
         {
            _Current = value;
         }
      }

      // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  -

      public new void Init()
      {
         // dummy
         throw new NotImplementedException();
      }

      public async new Task<IMediaItem> Play(string uri)
      {
         throw new NotImplementedException();
      }
   }
}
