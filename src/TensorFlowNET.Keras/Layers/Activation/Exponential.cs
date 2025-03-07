﻿using System;
using System.Collections.Generic;
using System.Text;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Engine;
using static Tensorflow.Binding;

namespace Tensorflow.Keras.Layers {
    public class Exponential : Layer
    {
        public Exponential(LayerArgs args) : base(args)
        {
            // Exponential has no args
        }
        public override void build(Shape input_shape)
        {
            _buildInputShape = input_shape;
            built = true;
        }
        protected override Tensors Call(Tensors inputs, Tensor state = null, bool? training = null)
        {
            Tensor output = inputs;
            return tf.exp(output);
        }
        public override Shape ComputeOutputShape(Shape input_shape)
        {
            return input_shape;
        }
    }
}
