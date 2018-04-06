using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeepNeuralNetwork {

    public static System.Random rnd;  

    private int nInput;  // number of input nodes
    private int[] nHidden;  // number of hidden nodes in each layer
    private int nOutput;  // number of output nodes
    private int nLayers;  // number of hidden node layers

    private double[] iNodes;  // input nodes
    private double[][] hNodes; // hidden nodes
    private double[] oNodes; // output nodes

    public double[][] ihWeights;  // input to 1st hidden layer weights
    public double[][][] hhWeights; // hidden to hidden layer weights
    public double[][] hoWeights;  // last hidden to output layer weights

    public double[][] hBiases;  // hidden node biases
    public double[] oBiases;  // output node biases


    public DeepNeuralNetwork(int numInput, int[] numHidden, int numOutput)
    {
        rnd = new System.Random((int)DateTime.Now.Ticks);  // random seed

        // initialize arrays 
        this.nInput = numInput;
        this.nHidden = new int[numHidden.Length];
        for (int i = 0; i < numHidden.Length; ++i)
            this.nHidden[i] = numHidden[i];
        this.nOutput = numOutput;
        this.nLayers = numHidden.Length;

        iNodes = new double[numInput];
        hNodes = MakeJaggedMatrix(numHidden);
        oNodes = new double[numOutput];

        ihWeights = MakeMatrix(numInput, numHidden[0]);
        hoWeights = MakeMatrix(numHidden[nLayers - 1], numOutput);

        hhWeights = new double[nLayers - 1][][];  // e.g. 3 hidden layers results in 2 hidden to hidden 
        for (int h = 0; h < hhWeights.Length; ++h)
        {
            int rows = numHidden[h];
            int cols = numHidden[h + 1];
            hhWeights[h] = MakeMatrix(rows, cols);
        }

        hBiases = MakeJaggedMatrix(numHidden);  
        oBiases = new double[numOutput];

        InitializeWeights(); // initializes all weights with random values
    }

    /// <summary>
    /// This method creates a 1D array containing random weight and bias values.
    /// A 1D Array makes storing the weights easier
    /// The SetWeights method will then take the created array and initialize all arrays with the values
    /// Order: ihweights - hhWeights[] - hoWeights - hBiases[] - oBiases
    /// </summary>
    public void InitializeWeights()
    {
        // make wts
        double lo = -0.75f;
        double hi = +0.75f;
        int numWts = DeepNeuralNetwork.NumWeights(this.nInput, this.nHidden, this.nOutput);
        double[] wts = new double[numWts];
        for (int i = 0; i < numWts; ++i)
            wts[i] = (hi - lo) * rnd.NextDouble() + lo;
        this.SetWeights(wts);
    }

    /// <summary>
    /// In this method a input will be fed in the network
    /// to calculate the result we will start in the first hidden layer
    /// the previous layer will be a 1D array - a Vector
    /// the current layer is a matrix 
    /// by multiplying these two togheter we will receive a weighted sum for each neuron
    /// we add the bias value to every single neuron and apply a activation function
    /// this process is repeated until we reach the output layer
    /// activation function used for every layer but the last one: hyperbolic tangent (values from -1 to 1)
    /// activation function used for ouput layer only: sigmoid (values from 0 to 1)
    /// </summary>
    /// <param name="xValues">1D Array that has to fit the size of the inputs array</param>
    /// <returns>Array with the length of the output layer contains values from 0 to 1</returns>
    public double[] ComputeOutputs(double[] xValues)
    {

        if (xValues.Length != nInput)
        {
            Debug.LogError("InputNodes and parameter length does not match");
        }

        // copy values into iNodes
        for (int i = 0; i < nInput; ++i) 
            iNodes[i] = xValues[i];

        // reset all hidden layers to 0.0
        for (int h = 0; h < nLayers; ++h)
            for (int j = 0; j < nHidden[h]; ++j)
                hNodes[h][j] = 0.0;

        // reset ouput layer to 0.0
        for (int k = 0; k < nOutput; ++k)
            oNodes[k] = 0.0;

        // input to 1st hidden layer
        for (int j = 0; j < nHidden[0]; ++j)  // each hidden node, 1st layer
        {
            for (int i = 0; i < nInput; ++i)
                hNodes[0][j] += ihWeights[i][j] * iNodes[i];
            // add the bias
            hNodes[0][j] += hBiases[0][j];
            // apply activation
            hNodes[0][j] = Math.Tanh(hNodes[0][j]);
        }

        // skip the already computed 1st hidden layer
        for (int h = 1; h < nLayers; ++h)
        {
            for (int j = 0; j < nHidden[h]; ++j)  // next layer that will be calculated
            {
                for (int jj = 0; jj < nHidden[h - 1]; ++jj)  // layer from which the next one will be calculated
                {
                    hNodes[h][j] += hhWeights[h - 1][jj][j] * hNodes[h - 1][jj];
                }
                hNodes[h][j] += hBiases[h][j];  // add bias value
                hNodes[h][j] = Math.Tanh(hNodes[h][j]);  // apply activation function
            }
        }

        // compute ouput node values
        for (int k = 0; k < nOutput; ++k)
        {
            for (int j = 0; j < nHidden[nLayers - 1]; ++j)
            {
                oNodes[k] += hoWeights[j][k] * hNodes[nLayers - 1][j];
            }
            oNodes[k] += oBiases[k];  // add bias value
        }

        double[] retResult = Softmax(oNodes);  // softmax activation all oNodes

        // copy retResult to oNodes because of missing softmax on oNodes
        for (int k = 0; k < nOutput; ++k)
            oNodes[k] = retResult[k];
        return retResult;  // return the ouputs only

    } // compute outputs

    /// <summary>
    /// takes in a double array and calculates the softmax for every index
    /// sigmoid activation function
    /// </summary>
    /// <param name="oSums">Double array</param>
    /// <returns></returns>
    private static double[] Softmax(double[] oSums)
    {
        // does all output nodes at once so scale
        // doesn't have to be re-computed each time.

        double sum = 0.0;
        for (int i = 0; i < oSums.Length; ++i)
            sum += Math.Exp(oSums[i]);

        double[] result = new double[oSums.Length];
        for (int i = 0; i < oSums.Length; ++i)
            result[i] = Math.Exp(oSums[i]) / sum;

        return result; // now scaled so that xi sum to 1.0
    }


    public void SetWeights(double[] wts)
    {
        // order: ihweights - hhWeights[] - hoWeights - hBiases[] - oBiases
        int nw = NumWeights(this.nInput, this.nHidden, this.nOutput);  // total num wts + biases
        if (wts.Length != nw)
        {
            Debug.LogError("Weights size does not match");
        }

        int ptr = 0;  // pointer into wts[]

        for (int i = 0; i < nInput; ++i)  // input node
            for (int j = 0; j < hNodes[0].Length; ++j)  // 1st hidden layer nodes
                ihWeights[i][j] = wts[ptr++];

        for (int h = 0; h < nLayers - 1; ++h)  // not last h layer
        {
            for (int j = 0; j < nHidden[h]; ++j)  // from node
            {
                for (int jj = 0; jj < nHidden[h + 1]; ++jj)  // to node
                {
                    hhWeights[h][j][jj] = wts[ptr++];
                }
            }
        }

        int hi = this.nLayers - 1;  // if 3 hidden layers (0,1,2) last is 3-1 = [2]
        for (int j = 0; j < this.nHidden[hi]; ++j)
        {
            for (int k = 0; k < this.nOutput; ++k)
            {
                hoWeights[j][k] = wts[ptr++];
            }
        }

        for (int h = 0; h < nLayers; ++h)  // hidden node biases
        {
            for (int j = 0; j < this.nHidden[h]; ++j)
            {
                hBiases[h][j] = wts[ptr++];
            }
        }

        for (int k = 0; k < nOutput; ++k)
        {
            oBiases[k] = wts[ptr++];
        }
    } // SetWeights

    public void updateWeights(double[] wts, double chance, double mutationRate)
    {
        // scale the weights with the factor
        double[] weights = this.GetWeights();
        double[] newWeights = new double[DeepNeuralNetwork.NumWeights(this.nInput, this.nHidden, this.nOutput)];

        for (int i = 0; i < wts.Length; i++)
        {
            if (rnd.Next(1,101) <= mutationRate)
            {
                int x = rnd.Next(1, 101);
                // 20% chance for one of these cases to occure
                if (x <= 20)
                {
                    newWeights[i] = (0.75f - -0.75f) * rnd.NextDouble() + -0.75f;
                }
                else if (x <= 40){
                    newWeights[i] += (0.375f - -0.375f) * rnd.NextDouble() + -0.375f;
                }
                else if (x <= 60)
                {
                    newWeights[i] -= (0.375f - -0.375f) * rnd.NextDouble() + -0.375f;
                }
                else if (x <= 80)
                {
                    newWeights[i] += (0.75f - -0.75f) * rnd.NextDouble() + -0.75f;
                } else
                {
                    newWeights[i] -= (0.75f - -0.75f) * rnd.NextDouble() + -0.75f;
                }
            }
            else
            {
                if (rnd.Next(1, 101) <= chance)
                {
                    newWeights[i] = wts[i];
                }
                else
                {
                    newWeights[i] = weights[i];
                }
            }            
        }

        // order: ihweights - hhWeights[] - hoWeights - hBiases[] - oBiases
        int nw = NumWeights(this.nInput, this.nHidden, this.nOutput);  // total num wts + biases
        if (wts.Length != nw)
            return;
        int ptr = 0;  // pointer into wts[]

        for (int i = 0; i < nInput; ++i)  // input node
            for (int j = 0; j < hNodes[0].Length; ++j)  // 1st hidden layer nodes
                ihWeights[i][j] = newWeights[ptr++];

        for (int h = 0; h < nLayers - 1; ++h)  // not last h layer
        {
            for (int j = 0; j < nHidden[h]; ++j)  // from node
            {
                for (int jj = 0; jj < nHidden[h + 1]; ++jj)  // to node
                {
                    hhWeights[h][j][jj] = newWeights[ptr++];
                }
            }
        }

        int hi = this.nLayers - 1;  // if 3 hidden layers (0,1,2) last is 3-1 = [2]
        for (int j = 0; j < this.nHidden[hi]; ++j)
        {
            for (int k = 0; k < this.nOutput; ++k)
            {
                hoWeights[j][k] = newWeights[ptr++];
            }
        }

        for (int h = 0; h < nLayers; ++h)  // hidden node biases
        {
            for (int j = 0; j < this.nHidden[h]; ++j)
            {
                hBiases[h][j] = newWeights[ptr++];
            }
        }

        for (int k = 0; k < nOutput; ++k)
        {
            oBiases[k] = newWeights[ptr++];
        }
    } // updateWeights

    public double[] GetWeights()
    {
        // order: ihweights -> hhWeights[] -> hoWeights -> hBiases[] -> oBiases
        int nw = NumWeights(this.nInput, this.nHidden, this.nOutput);  // total num wts + biases
        double[] result = new double[nw];

        int ptr = 0;  // pointer into result[]

        for (int i = 0; i < nInput; ++i)  // input node
            for (int j = 0; j < hNodes[0].Length; ++j)  // 1st hidden layer nodes
                result[ptr++] = ihWeights[i][j];

        for (int h = 0; h < nLayers - 1; ++h)  // not last h layer
        {
            for (int j = 0; j < nHidden[h]; ++j)  // from node
            {
                for (int jj = 0; jj < nHidden[h + 1]; ++jj)  // to node
                {
                    result[ptr++] = hhWeights[h][j][jj];
                }
            }
        }

        int hi = this.nLayers - 1;  // if 3 hidden layers (0,1,2) last is 3-1 = [2]
        for (int j = 0; j < this.nHidden[hi]; ++j)
        {
            for (int k = 0; k < this.nOutput; ++k)
            {
                result[ptr++] = hoWeights[j][k];
            }
        }

        for (int h = 0; h < nLayers; ++h)  // hidden node biases
        {
            for (int j = 0; j < this.nHidden[h]; ++j)
            {
                result[ptr++] = hBiases[h][j];
            }
        }

        for (int k = 0; k < nOutput; ++k)
        {
            result[ptr++] = oBiases[k];
        }
        return result;
    }

    public static int NumWeights(int numInput, int[] numHidden, int numOutput)
    {
        // total num weights and biases
        int ihWts = numInput * numHidden[0];

        int hhWts = 0;
        for (int j = 0; j < numHidden.Length - 1; ++j)
        {
            int rows = numHidden[j];
            int cols = numHidden[j + 1];
            hhWts += rows * cols;
        }
        int hoWts = numHidden[numHidden.Length - 1] * numOutput;

        int hbs = 0;
        for (int i = 0; i < numHidden.Length; ++i)
            hbs += numHidden[i];

        int obs = numOutput;
        int nw = ihWts + hhWts + hoWts + hbs + obs;
        return nw;
    }

    public static double[][] MakeJaggedMatrix(int[] cols)
    {
        // array of arrays using size info in cols[]
        int rows = cols.Length;  // num rows inferred by col count
        double[][] result = new double[rows][];
        for (int i = 0; i < rows; ++i)
        {
            int ncol = cols[i];
            result[i] = new double[ncol];
        }
        return result;
    }

    public static double[][] MakeMatrix(int rows, int cols)
    {
        double[][] result = new double[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new double[cols];
        return result;
    }
  //********

// Use this for initialization
void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
