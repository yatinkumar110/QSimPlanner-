﻿using QSP.LandingPerfCalculation;
using System.Windows.Forms;
using System;

namespace QSP.UI.Forms.LandingPerf.FormControllers
{
    public class FormController
    {
        protected PerfTable acPerf;
        protected LandingPerfElements elements;

        public FormController(PerfTable acPerf, LandingPerfElements elements)
        {
            this.acPerf = acPerf;
            this.elements = elements;
        }

        public virtual void AirportChanged(object sender, EventArgs e) { }

        public virtual void RunwayChanged(object sender, EventArgs e) { }

        public virtual void LengthUnitChanged(object sender, EventArgs e) { }

        public virtual void TempUnitChanged(object sender, EventArgs e) { }

        public virtual void PressureUnitChanged(object sender, EventArgs e) { }

        public virtual void SurfCondChanged(object sender, EventArgs e) { }

        public virtual void WeightUnitChanged(object sender, EventArgs e) { }

        public virtual void FlapsChanged(object sender, EventArgs e) { }

        public virtual void ReverserChanged(object sender, EventArgs e) { }

        public virtual void BrakesChanged(object sender, EventArgs e) { }

        public virtual void Compute(object sender, EventArgs e) { }
    }
}
