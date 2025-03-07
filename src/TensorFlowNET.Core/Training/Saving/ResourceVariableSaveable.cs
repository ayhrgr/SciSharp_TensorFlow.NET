﻿/*****************************************************************************
   Copyright 2018 The TensorFlow.NET Authors. All Rights Reserved.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
******************************************************************************/

using static Tensorflow.Binding;

namespace Tensorflow
{
    public class ResourceVariableSaveable : MySaveableObject
    {
        string _var_device;
        int[] _var_shape;
        Tensor handle_op;

        public ResourceVariableSaveable(Tensor var, string slice_spec, string name)
        {
            _var_device = var.Device;
            _var_shape = var.shape;
            handle_op = var.op.inputs[0];
            var tensor = var;
            var spec = new SaveSpec(tensor, slice_spec, name, dtype: var.dtype);

            op = var;
            specs = new SaveSpec[] { spec };
            this.name = name;
        }

        public ResourceVariableSaveable(BaseResourceVariable var, string slice_spec, string name)
        {
            _var_device = var.Device;
            _var_shape = var.shape;

            Tensor _read_variable_closure(BaseResourceVariable v)
            {
                tf.device(v.Device);
                if(tf.Context.executing_eagerly() && !((bool)v.is_initialized().numpy()))
                {
                    return null;
                }
                var x = v.read_value_no_copy();
                tf.device("/device:CPU:0");
                return array_ops.identity(x);
            }

            this.handle_op = var.Handle;
            var tensor = _read_variable_closure(var);

            var spec = new SaveSpec(tensor, slice_spec, name, dtype: var.dtype);
            _op = var;
            specs = new SaveSpec[] { spec };
            this.name = name;
        }

        public override Operation restore(Tensor[] restored_tensors, Shape[] restored_shapes = null)
        {
            var restored_tensor = restored_tensors[0];
            restored_tensor = array_ops.identity(restored_tensor);
            return resource_variable_ops.shape_safe_assign_variable_handle(
                handle_op, _var_shape, restored_tensor);
        }
    }
}
