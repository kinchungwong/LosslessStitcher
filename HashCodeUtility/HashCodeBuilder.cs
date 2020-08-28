using System;
using System.Collections.Generic;
using BitwiseUtility;

namespace HashCodeUtility
{
    public class HashCodeBuilder
    {
        private const uint _c1 = 0xcc9e2d51u;
        private const uint _c2 = 0x1b873593u;
        private const uint _c3 = 0xe6546b64u;
        private uint _state;

        public HashCodeBuilder(uint state)
        {
            _state = state;
        }

        public HashCodeBuilder(Type type)
            : this(_U(type.GetHashCode()))
        {
        }

        public static HashCodeBuilder ForType<T>()
        {
            return new HashCodeBuilder(typeof(T));
        }

        public HashCodeBuilder Ingest(uint data)
        {
            _UpdateState(_TransformData(data));
            return this;
        }

        public HashCodeBuilder Ingest(uint data0, uint data1)
        {
            _UpdateState(_TransformData(data0));
            _UpdateState(_TransformData(data1));
            return this;
        }

        public HashCodeBuilder Ingest(uint data0, uint data1, uint data2)
        {
            _UpdateState(_TransformData(data0));
            _UpdateState(_TransformData(data1));
            _UpdateState(_TransformData(data2));
            return this;
        }

        public HashCodeBuilder Ingest(uint data0, uint data1, uint data2, uint data3)
        {
            _UpdateState(_TransformData(data0));
            _UpdateState(_TransformData(data1));
            _UpdateState(_TransformData(data2));
            _UpdateState(_TransformData(data3));
            return this;
        }

        public HashCodeBuilder Ingest(uint data0, uint data1, uint data2, uint data3, uint data4)
        {
            _UpdateState(_TransformData(data0));
            _UpdateState(_TransformData(data1));
            _UpdateState(_TransformData(data2));
            _UpdateState(_TransformData(data3));
            _UpdateState(_TransformData(data4));
            return this;
        }

        public HashCodeBuilder Ingest(int data)
        {
            Ingest(_U(data));
            return this;
        }

        public HashCodeBuilder Ingest(int data0, int data1)
        {
            Ingest(_U(data0));
            Ingest(_U(data1));
            return this;
        }

        public HashCodeBuilder Ingest(int data0, int data1, int data2)
        {
            Ingest(_U(data0));
            Ingest(_U(data1));
            Ingest(_U(data2));
            return this;
        }

        public HashCodeBuilder Ingest(int data0, int data1, int data2, int data3)
        {
            Ingest(_U(data0));
            Ingest(_U(data1));
            Ingest(_U(data2));
            Ingest(_U(data3));
            return this;
        }

        public HashCodeBuilder Ingest(int data0, int data1, int data2, int data3, int data4)
        {
            Ingest(_U(data0));
            Ingest(_U(data1));
            Ingest(_U(data2));
            Ingest(_U(data3));
            Ingest(_U(data4));
            return this;
        }

        public HashCodeBuilder Ingest(ulong data)
        {
            uint data0, data1;
            unchecked
            {
                data0 = (uint)data;
                data1 = (uint)(data >> 32);
            }
            Ingest(data0, data1);
            return this;
        }

        public HashCodeBuilder Ingest(long data)
        {
            Ingest(unchecked((ulong)data));
            return this;
        }

        public HashCodeBuilder Ingest(ushort data)
        {
            Ingest((uint)data);
            return this;
        }

        public HashCodeBuilder Ingest(short data)
        {
            Ingest((int)data);
            return this;
        }

        public HashCodeBuilder Ingest(byte data)
        {
            Ingest((uint)data);
            return this;
        }

        public HashCodeBuilder Ingest(sbyte data)
        {
            Ingest((int)data);
            return this;
        }

        public HashCodeBuilder Ingest<T>(T data)
        {
            Ingest(data.GetHashCode());
            return this;
        }

        public HashCodeBuilder Ingest<T>(IEnumerable<T> arr)
        {
            foreach (T t in arr)
            {
                Ingest(t);
            }
            return this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                uint endstate7 = _state ^ (_state >> 16);
                uint endstate8 = endstate7 * 0x85ebca6bu;
                uint endstate9 = endstate8 ^ (endstate8 >> 13);
                uint endstate10 = endstate9 * 0xc2b2ae35u;
                uint endstate11 = endstate10 ^ (endstate10 >> 16);
                return (int)endstate11;
            }
        }

        private uint _TransformData(uint data)
        {
            unchecked
            {
                uint data2 = data * _c1;
                uint data3 = BitwiseOps.Rotate(data2, 15);
                uint data4 = data3 * _c2;
                return data4;
            }
        }

        private void _UpdateState(uint transformedData)
        {
            unchecked
            {
                uint state5 = _state ^ transformedData;
                uint state6 = BitwiseOps.Rotate(state5, 13);
                uint state7 = state6 * 5u + _c3;
                _state = state7;
            }
        }

        private static uint _U(int value)
        {
            return unchecked((uint)value);
        }
    }
}
