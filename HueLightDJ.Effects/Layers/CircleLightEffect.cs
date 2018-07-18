using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Circle Light")]
  public class CircleLightEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(0, 0));

      var customWaitMS = (waitTime.Value.Value.TotalMilliseconds * 2) / layer.Count;

      return orderedByAngle.To2DGroup().Flash(cancellationToken, color, IteratorEffectMode.Cycle, waitTime: TimeSpan.FromMilliseconds(customWaitMS), transitionTimeOn: TimeSpan.FromMilliseconds(customWaitMS / 2), transitionTimeOff: TimeSpan.FromMilliseconds(customWaitMS * 2), waitTillFinished: false);

    }
  }
}
