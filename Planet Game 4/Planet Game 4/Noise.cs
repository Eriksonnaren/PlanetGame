using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class Noise
    {
        public int Amount;//amount of nodes in the base wave
        public int Layers;//how many waves, amount is doubled each wave
        public double Scalar;//how much smaller each wave is, range 0,1
        public double TotalScalar;//scale the result to be in range 0,1
        public Random Rand;//local rng for special seeds
        double[][] Waves;
        /// <summary>
        /// Generates random noise
        /// </summary>
        /// <param name="Amount"> How many arraypoints are generated, more points is like zooming out</param>
        /// <param name="Layers"> How many noisewaves are stacked ontop of each other, more allows further zoom in before it get regular</param>
        /// <param name="Scalar"> How the waves scale with deeper waves, should be between 0 and 1, default =0.5</param>
        /// <param name="Seed"> The seed for the random number generator, 0=random</param>
        public Noise(int Amount,int Layers,double Scalar,int Seed)
        {
            if (Seed == 0)
                Rand = new Random(Form1.rnd.Next());//uses the rng from form1 to avoid duplicate seeds when multiple noise is created at the same time
            else
                Rand = new Random(Seed);
            if (Scalar >= 1 || Scalar < 0)
                throw new Exception("Scalar must be between 0 and 1");
            TotalScalar = (1 - Scalar) / (1 - intPow(Scalar, Layers));
            this.Amount = Amount;
            this.Layers = Layers;
            this.Scalar = Scalar;
            Waves= new double[Layers][];
            for (int i = 0; i <Layers; i++)
            {
                int A = Amount * (1<<i)+1;
                Waves[i] = new double[A];
                for (int i2 = 0; i2 < A-1; i2++)
                {
                    Waves[i][i2] = 2*Rand.NextDouble()-1;
                    //Waves[i][i2] = (i2&1)*2-1;
                }
                Waves[i][A - 1] = Waves[i][0];
            }
        }
        /// <summary>
        /// get a noise value between -1 and 1
        /// </summary>
        /// <param name="x"> position along the wave, should be between 0 and 1</param>
        /// <returns> returns the amplitude of the wave</returns>
        public double Get(double x)
        {
            if (x >= 1 || x < 0)
                throw new Exception("x must be between 0 and 1");
            double Value = 0;
            double S=1;
            for (int i = 0; i < Layers; i++)
            {
                Value += getWave(i,x)*S;
                S *= Scalar;
            }
            return Value * TotalScalar;
        }
        double getWave(int id,double x)
        {
            double[] Wave = Waves[id];//current wave
            double X = x * (Wave.Length-1);//scales x to match wave
            int x0 = (int)X;//get the lower point
            int x1 = x0 + 1;//get the upper point, x is between x0 and x1
            double dot0 = (X - x0) * Wave[x0];//dot product of lower point
            double dot1 = (X - x1) * Wave[x1];//dot product of upper point
            double t = Curve(X - x0);//get the lerp value
            double result = lerp(dot0, dot1, t);//get result
            return result*2;//needs to be doubled since it is 0.5 by default
            
        }
        double lerp(double a,double b,double t)//regular lerp
        {
            return a+(b-a)*t;
        }
        double Curve(double x)//get bezier curve to smooth the lerp at the curvepoints. ie make the derivative 0 at 0 and 1, plug into graph calculator to see what I mean ;)
        {
            double x2 = x * x;//x^2
            double x3 = x2 * x;//x^3
            return (3 * x2) - (2 * x3);//3x^2-2x^3
        }
        double intPow(double x,int y)
        {
            double S = 1;
            for (int i = 0; i < y; i++)
            {
                S *= x;
            }
            return S;
        }
    }
}
