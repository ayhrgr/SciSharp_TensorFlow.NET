﻿using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Saving;

namespace Tensorflow.Keras.ArgsDefinition
{
    public class DataAdapterArgs: IKerasConfig
    {
        public Tensor X { get; set; }
        public Tensor Y { get; set; }
        public IDatasetV2 Dataset { get; set; }
        public int BatchSize { get; set; } = 32;
        public int Steps { get; set; }
        public int Epochs { get; set; }
        public bool Shuffle { get; set; }
        public int MaxQueueSize { get; set; }
        public int Worker { get; set; }
        public bool UseMultiprocessing { get; set; }
        public IModel Model { get; set; }
    }
}
