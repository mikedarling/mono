//
// SslStreamTest.cs
//      - Unit tests for System.Net.Security.SslStream
//
// Authors:
//      Maciej Paszta (maciej.paszta@gmail.com)
//      Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright (C) Maciej Paszta, 2012
// Copyright 2014 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MonoTests.System.Net.Security
{

[TestFixture]
public class SslStreamTest {

	byte[] m_serverCertRaw = { 48, 130, 5, 165, 2, 1, 3, 48, 130, 5, 95, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 160, 130, 5, 80, 4, 130, 5, 76, 48, 130, 5, 72, 48, 130, 2, 87, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 6, 160, 130, 2, 72, 48, 130, 2, 68, 2, 1, 0, 48, 130, 2, 61, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 48, 28, 6, 10, 42, 134, 72, 134, 247, 13, 1, 12, 1, 3, 48, 14, 4, 8, 211, 176, 234, 3, 252, 26, 32, 15, 2, 2, 7, 208, 128, 130, 2, 16, 183, 149, 35, 180, 127, 95, 163, 122, 138, 244, 29, 177, 220, 173, 46, 73, 208, 217, 211, 190, 164, 183, 21, 110, 33, 122, 98, 163, 251, 16, 23, 106, 154, 14, 52, 177, 3, 12, 248, 226, 48, 123, 211, 6, 216, 6, 192, 175, 203, 142, 141, 143, 252, 178, 7, 162, 81, 232, 159, 42, 56, 177, 191, 53, 7, 146, 189, 236, 75, 140, 210, 143, 11, 103, 64, 58, 10, 73, 123, 39, 97, 119, 166, 114, 123, 65, 68, 214, 42, 17, 156, 122, 8, 58, 184, 134, 255, 48, 64, 20, 229, 247, 196, 12, 130, 56, 176, 69, 179, 254, 216, 45, 25, 244, 240, 116, 88, 137, 66, 13, 18, 202, 199, 59, 200, 245, 19, 175, 232, 217, 211, 12, 191, 222, 26, 162, 253, 73, 201, 48, 61, 3, 248, 117, 16, 71, 233, 183, 90, 110, 91, 116, 56, 133, 223, 148, 19, 78, 140, 123, 159, 203, 78, 15, 172, 39, 190, 39, 71, 180, 155, 48, 156, 116, 212, 52, 1, 231, 201, 196, 73, 87, 68, 104, 208, 40, 104, 32, 218, 235, 245, 84, 136, 168, 51, 9, 93, 126, 46, 80, 180, 240, 144, 79, 88, 87, 159, 24, 108, 186, 9, 20, 48, 100, 148, 250, 4, 163, 115, 131, 44, 13, 38, 222, 117, 196, 196, 128, 114, 149, 97, 93, 37, 191, 3, 192, 231, 88, 80, 218, 147, 8, 192, 165, 27, 206, 56, 42, 157, 230, 223, 130, 253, 169, 182, 245, 192, 181, 18, 212, 133, 168, 73, 92, 66, 197, 117, 245, 107, 127, 23, 146, 249, 41, 66, 219, 210, 207, 221, 205, 205, 15, 110, 92, 12, 207, 76, 239, 4, 13, 129, 127, 170, 205, 253, 148, 208, 24, 129, 24, 210, 220, 85, 45, 179, 137, 66, 134, 142, 22, 112, 48, 160, 236, 232, 38, 83, 101, 55, 51, 18, 110, 99, 69, 41, 173, 107, 233, 11, 199, 23, 61, 135, 222, 94, 74, 29, 219, 80, 128, 167, 186, 254, 235, 42, 96, 134, 5, 13, 90, 59, 231, 137, 195, 207, 28, 165, 12, 218, 5, 72, 102, 61, 135, 198, 73, 250, 97, 89, 214, 179, 244, 194, 23, 142, 157, 4, 243, 90, 69, 54, 10, 139, 76, 95, 40, 225, 219, 59, 15, 54, 182, 206, 142, 228, 248, 79, 156, 129, 246, 63, 6, 6, 236, 44, 67, 116, 213, 170, 47, 193, 186, 139, 25, 80, 166, 57, 99, 231, 156, 191, 117, 65, 76, 7, 243, 244, 127, 225, 210, 190, 164, 141, 46, 36, 99, 111, 203, 133, 127, 80, 28, 61, 160, 36, 132, 182, 16, 41, 39, 185, 232, 123, 32, 57, 189, 100, 152, 38, 205, 5, 189, 240, 65, 3, 191, 73, 85, 12, 209, 180, 1, 194, 70, 124, 57, 71, 48, 230, 235, 122, 175, 157, 35, 233, 83, 40, 20, 169, 224, 14, 11, 216, 48, 194, 105, 25, 187, 210, 182, 6, 184, 73, 95, 85, 210, 227, 113, 58, 10, 186, 175, 254, 25, 102, 39, 3, 2, 200, 194, 197, 200, 224, 77, 164, 8, 36, 114, 48, 130, 2, 233, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 160, 130, 2, 218, 4, 130, 2, 214, 48, 130, 2, 210, 48, 130, 2, 206, 6, 11, 42, 134, 72, 134, 247, 13, 1, 12, 10, 1, 2, 160, 130, 2, 166, 48, 130, 2, 162, 48, 28, 6, 10, 42, 134, 72, 134, 247, 13, 1, 12, 1, 3, 48, 14, 4, 8, 178, 13, 52, 135, 85, 49, 79, 105, 2, 2, 7, 208, 4, 130, 2, 128, 21, 84, 227, 109, 230, 144, 140, 170, 117, 250, 179, 207, 129, 100, 126, 126, 29, 231, 94, 140, 45, 26, 168, 45, 240, 4, 170, 73, 98, 115, 109, 96, 177, 206, 6, 80, 170, 22, 237, 144, 58, 95, 59, 26, 85, 135, 178, 69, 184, 44, 122, 81, 213, 135, 149, 198, 246, 83, 68, 129, 2, 186, 118, 33, 44, 214, 227, 240, 220, 51, 175, 220, 220, 180, 113, 216, 101, 138, 81, 54, 38, 0, 216, 30, 29, 187, 213, 230, 12, 181, 130, 21, 241, 98, 120, 41, 150, 176, 69, 37, 169, 249, 123, 212, 254, 135, 154, 214, 127, 39, 105, 149, 180, 218, 41, 207, 75, 70, 105, 169, 185, 169, 132, 173, 188, 82, 251, 71, 234, 136, 5, 254, 110, 223, 34, 4, 145, 7, 19, 51, 123, 140, 75, 226, 0, 21, 220, 228, 223, 218, 8, 169, 210, 194, 139, 93, 218, 55, 40, 174, 50, 238, 38, 166, 222, 103, 0, 209, 88, 131, 51, 222, 154, 217, 18, 172, 73, 17, 133, 54, 173, 208, 118, 104, 167, 113, 153, 223, 251, 154, 120, 176, 18, 127, 51, 206, 164, 77, 86, 9, 82, 212, 86, 162, 206, 230, 79, 217, 178, 42, 217, 162, 152, 188, 217, 59, 212, 117, 200, 135, 75, 74, 43, 1, 42, 79, 180, 164, 250, 122, 103, 103, 157, 11, 14, 33, 48, 8, 108, 155, 46, 124, 223, 204, 169, 124, 104, 11, 246, 213, 226, 16, 125, 17, 228, 15, 178, 141, 79, 78, 115, 76, 131, 122, 166, 124, 154, 1, 174, 178, 176, 213, 208, 188, 71, 118, 220, 168, 64, 218, 176, 134, 38, 229, 14, 109, 162, 125, 16, 57, 249, 201, 180, 17, 182, 143, 184, 12, 248, 113, 65, 70, 109, 79, 249, 34, 170, 35, 228, 219, 121, 202, 228, 121, 127, 255, 22, 173, 202, 171, 33, 232, 4, 240, 142, 216, 80, 56, 177, 83, 93, 123, 217, 213, 157, 99, 34, 194, 61, 228, 239, 194, 20, 27, 9, 53, 132, 79, 19, 97, 107, 31, 51, 39, 176, 223, 90, 88, 67, 138, 194, 169, 176, 144, 202, 119, 146, 74, 27, 118, 63, 129, 230, 101, 104, 75, 116, 49, 223, 254, 225, 70, 206, 183, 11, 134, 148, 10, 55, 57, 50, 178, 144, 164, 139, 233, 169, 109, 186, 211, 95, 123, 75, 111, 192, 187, 127, 240, 45, 226, 194, 240, 128, 10, 79, 178, 192, 66, 21, 197, 24, 171, 141, 255, 185, 230, 84, 206, 151, 9, 93, 115, 162, 12, 115, 129, 218, 103, 219, 183, 142, 123, 3, 110, 139, 208, 4, 146, 76, 99, 246, 240, 32, 169, 148, 16, 146, 172, 230, 36, 56, 145, 23, 94, 209, 92, 38, 244, 127, 70, 121, 253, 66, 55, 36, 140, 98, 105, 233, 112, 24, 23, 230, 112, 62, 244, 12, 48, 30, 51, 0, 18, 244, 139, 66, 245, 234, 203, 195, 52, 119, 255, 84, 82, 204, 100, 176, 167, 24, 224, 8, 127, 214, 148, 115, 242, 56, 190, 72, 221, 68, 252, 36, 74, 254, 57, 52, 96, 20, 173, 32, 236, 87, 15, 16, 76, 9, 48, 3, 61, 2, 137, 137, 9, 68, 213, 99, 163, 63, 201, 83, 241, 98, 7, 117, 108, 4, 123, 170, 18, 10, 19, 198, 31, 170, 15, 247, 216, 145, 172, 239, 137, 181, 80, 160, 24, 11, 35, 131, 58, 218, 22, 250, 215, 52, 160, 246, 197, 183, 92, 137, 0, 245, 63, 49, 183, 246, 195, 58, 63, 4, 75, 10, 92, 131, 181, 59, 78, 247, 44, 150, 49, 49, 107, 211, 62, 71, 62, 222, 159, 161, 118, 236, 55, 219, 49, 0, 3, 82, 236, 96, 20, 83, 39, 245, 208, 240, 245, 174, 218, 49, 21, 48, 19, 6, 9, 42, 134, 72, 134, 247, 13, 1, 9, 21, 49, 6, 4, 4, 1, 0, 0, 0, 48, 61, 48, 33, 48, 9, 6, 5, 43, 14, 3, 2, 26, 5, 0, 4, 20, 30, 154, 48, 126, 198, 239, 114, 62, 12, 58, 129, 172, 67, 156, 76, 214, 62, 205, 89, 28, 4, 20, 135, 177, 105, 83, 79, 93, 181, 149, 169, 49, 112, 201, 70, 212, 153, 79, 198, 163, 137, 90, 2, 2, 7, 208 };
	byte[] m_clientCertRaw = { 48, 130, 5, 173, 2, 1, 3, 48, 130, 5, 103, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 160, 130, 5, 88, 4, 130, 5, 84, 48, 130, 5, 80, 48, 130, 2, 95, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 6, 160, 130, 2, 80, 48, 130, 2, 76, 2, 1, 0, 48, 130, 2, 69, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 48, 28, 6, 10, 42, 134, 72, 134, 247, 13, 1, 12, 1, 3, 48, 14, 4, 8, 35, 249, 113, 131, 30, 42, 21, 176, 2, 2, 7, 208, 128, 130, 2, 24, 78, 185, 144, 242, 231, 15, 133, 251, 122, 86, 61, 132, 148, 253, 47, 83, 198, 14, 11, 70, 79, 14, 21, 66, 91, 72, 147, 159, 95, 245, 240, 210, 194, 174, 25, 112, 171, 126, 126, 143, 64, 173, 63, 224, 49, 172, 100, 129, 84, 86, 91, 50, 28, 29, 118, 139, 22, 251, 248, 181, 110, 246, 226, 92, 108, 178, 25, 199, 62, 90, 12, 5, 189, 249, 22, 230, 37, 230, 190, 97, 50, 12, 252, 4, 66, 204, 92, 12, 98, 222, 69, 230, 221, 64, 163, 106, 194, 113, 223, 40, 81, 138, 123, 212, 171, 160, 178, 153, 29, 108, 64, 110, 166, 82, 26, 157, 63, 69, 66, 93, 231, 232, 228, 189, 85, 63, 11, 53, 192, 171, 124, 148, 0, 31, 106, 146, 207, 71, 16, 138, 214, 79, 0, 103, 133, 199, 116, 45, 127, 230, 199, 230, 11, 179, 9, 253, 45, 23, 194, 122, 217, 20, 200, 214, 127, 138, 133, 190, 29, 110, 129, 29, 20, 186, 106, 182, 114, 134, 120, 170, 120, 137, 111, 200, 137, 10, 43, 139, 183, 217, 245, 38, 165, 126, 142, 233, 20, 238, 238, 185, 12, 71, 4, 54, 128, 28, 70, 139, 94, 119, 25, 243, 241, 161, 125, 97, 132, 19, 225, 249, 117, 226, 108, 58, 163, 221, 126, 111, 192, 157, 65, 104, 134, 83, 92, 26, 143, 23, 112, 12, 94, 111, 59, 138, 79, 93, 98, 49, 239, 77, 99, 119, 89, 127, 176, 12, 217, 67, 46, 84, 74, 10, 63, 227, 18, 153, 118, 104, 92, 31, 198, 187, 91, 139, 239, 231, 154, 111, 254, 75, 172, 166, 87, 251, 152, 231, 61, 101, 115, 121, 190, 52, 95, 195, 134, 176, 248, 143, 13, 145, 141, 107, 166, 175, 231, 243, 27, 105, 150, 61, 179, 89, 134, 182, 140, 243, 116, 170, 255, 110, 26, 137, 79, 102, 45, 225, 160, 67, 75, 19, 58, 188, 168, 11, 98, 149, 139, 164, 93, 236, 115, 245, 59, 183, 177, 3, 115, 218, 35, 117, 62, 172, 172, 179, 230, 209, 116, 119, 41, 144, 90, 242, 74, 107, 153, 130, 250, 38, 236, 33, 11, 117, 51, 42, 213, 15, 24, 57, 193, 250, 76, 41, 79, 229, 249, 215, 236, 131, 136, 160, 186, 142, 7, 70, 197, 21, 148, 57, 136, 70, 89, 15, 157, 231, 130, 24, 80, 99, 64, 144, 75, 210, 255, 101, 51, 200, 237, 180, 238, 195, 173, 187, 225, 177, 212, 99, 176, 28, 51, 33, 37, 230, 79, 112, 142, 174, 75, 183, 125, 207, 108, 88, 9, 76, 173, 254, 165, 193, 97, 39, 245, 80, 0, 131, 225, 116, 179, 67, 168, 171, 143, 11, 49, 153, 244, 185, 253, 9, 42, 40, 53, 225, 137, 184, 37, 31, 53, 121, 28, 140, 27, 145, 84, 182, 40, 176, 152, 135, 77, 232, 20, 144, 74, 81, 227, 29, 26, 179, 50, 80, 244, 181, 54, 146, 224, 25, 233, 70, 0, 153, 227, 72, 140, 142, 185, 141, 177, 127, 252, 107, 240, 146, 255, 122, 194, 92, 147, 69, 52, 67, 124, 144, 207, 146, 182, 131, 48, 130, 2, 233, 6, 9, 42, 134, 72, 134, 247, 13, 1, 7, 1, 160, 130, 2, 218, 4, 130, 2, 214, 48, 130, 2, 210, 48, 130, 2, 206, 6, 11, 42, 134, 72, 134, 247, 13, 1, 12, 10, 1, 2, 160, 130, 2, 166, 48, 130, 2, 162, 48, 28, 6, 10, 42, 134, 72, 134, 247, 13, 1, 12, 1, 3, 48, 14, 4, 8, 46, 213, 31, 185, 121, 55, 235, 182, 2, 2, 7, 208, 4, 130, 2, 128, 62, 51, 182, 78, 208, 241, 24, 1, 167, 56, 187, 181, 138, 26, 252, 10, 43, 143, 17, 4, 102, 205, 177, 108, 52, 174, 60, 135, 233, 89, 184, 112, 5, 43, 87, 209, 148, 146, 224, 83, 167, 26, 165, 130, 202, 139, 251, 183, 156, 167, 251, 209, 127, 169, 91, 124, 18, 171, 5, 47, 145, 51, 113, 161, 84, 123, 26, 149, 11, 79, 8, 14, 242, 162, 215, 239, 51, 120, 85, 183, 144, 208, 130, 198, 4, 98, 217, 54, 29, 168, 103, 60, 50, 72, 92, 160, 51, 107, 153, 40, 15, 143, 75, 78, 212, 77, 206, 188, 176, 134, 213, 101, 109, 116, 238, 215, 26, 90, 33, 134, 160, 56, 21, 200, 6, 27, 185, 239, 8, 193, 188, 61, 114, 101, 76, 224, 75, 28, 18, 149, 83, 33, 100, 103, 59, 246, 21, 236, 141, 241, 126, 163, 126, 236, 180, 106, 98, 6, 196, 11, 19, 12, 81, 153, 79, 221, 230, 199, 176, 95, 8, 124, 189, 242, 151, 182, 126, 250, 227, 53, 55, 86, 39, 85, 171, 57, 157, 14, 215, 226, 204, 195, 59, 121, 85, 54, 213, 45, 101, 164, 38, 112, 114, 168, 20, 28, 152, 139, 43, 146, 15, 84, 64, 46, 39, 55, 56, 110, 160, 32, 120, 156, 253, 64, 79, 163, 3, 156, 85, 80, 197, 214, 26, 250, 200, 63, 212, 4, 119, 96, 32, 25, 1, 121, 112, 170, 87, 75, 163, 32, 175, 195, 82, 64, 74, 247, 4, 152, 203, 18, 129, 201, 221, 98, 35, 84, 148, 57, 15, 121, 90, 195, 79, 50, 99, 73, 163, 162, 131, 26, 203, 106, 237, 135, 203, 239, 43, 253, 187, 68, 33, 82, 101, 121, 61, 9, 223, 54, 67, 138, 11, 146, 175, 102, 163, 112, 51, 63, 124, 248, 183, 89, 81, 250, 15, 159, 161, 201, 38, 6, 243, 224, 61, 143, 117, 144, 157, 184, 242, 248, 155, 150, 17, 13, 158, 1, 91, 33, 107, 65, 106, 153, 211, 18, 7, 138, 230, 8, 84, 56, 110, 227, 0, 47, 33, 181, 141, 185, 119, 93, 72, 192, 100, 76, 145, 40, 163, 185, 96, 154, 151, 172, 86, 249, 167, 237, 97, 28, 137, 27, 127, 114, 218, 49, 106, 92, 40, 201, 252, 219, 52, 129, 17, 105, 198, 29, 166, 30, 229, 103, 216, 102, 84, 146, 210, 114, 32, 186, 205, 252, 253, 142, 103, 75, 83, 122, 72, 42, 118, 210, 41, 113, 227, 206, 27, 79, 83, 5, 31, 201, 245, 165, 18, 210, 112, 215, 144, 78, 91, 84, 3, 61, 236, 192, 152, 78, 16, 254, 242, 67, 46, 228, 98, 102, 20, 2, 43, 134, 97, 180, 17, 189, 30, 214, 167, 32, 128, 106, 61, 227, 166, 41, 81, 51, 208, 245, 114, 147, 66, 34, 212, 35, 152, 26, 173, 133, 0, 207, 88, 5, 171, 175, 5, 75, 207, 50, 153, 141, 141, 2, 47, 236, 252, 132, 87, 173, 163, 208, 119, 213, 77, 58, 145, 12, 21, 40, 4, 23, 114, 204, 89, 136, 152, 123, 159, 205, 149, 51, 21, 146, 219, 75, 25, 199, 22, 210, 203, 66, 19, 10, 188, 98, 152, 60, 161, 234, 122, 109, 232, 197, 79, 77, 185, 80, 210, 87, 120, 232, 158, 103, 124, 88, 110, 47, 4, 67, 123, 72, 230, 160, 33, 1, 146, 163, 54, 149, 79, 54, 21, 124, 163, 210, 38, 150, 176, 100, 53, 56, 220, 190, 98, 203, 250, 122, 34, 213, 17, 101, 203, 37, 231, 176, 182, 65, 196, 42, 169, 76, 21, 27, 87, 95, 88, 82, 52, 234, 179, 82, 207, 62, 185, 251, 85, 225, 70, 245, 220, 29, 177, 64, 146, 94, 216, 101, 226, 10, 116, 209, 44, 10, 49, 21, 48, 19, 6, 9, 42, 134, 72, 134, 247, 13, 1, 9, 21, 49, 6, 4, 4, 1, 0, 0, 0, 48, 61, 48, 33, 48, 9, 6, 5, 43, 14, 3, 2, 26, 5, 0, 4, 20, 200, 225, 57, 176, 214, 252, 236, 240, 126, 231, 49, 34, 77, 228, 178, 235, 151, 135, 242, 52, 4, 20, 251, 225, 117, 50, 254, 96, 240, 190, 40, 228, 34, 104, 253, 203, 163, 169, 25, 46, 239, 103, 2, 2, 7, 208 };	
	X509Certificate2 m_serverCert;
	X509Certificate2 m_clientCert;

	[SetUp]
	public void GetReady () {
		m_serverCert = new X509Certificate2 (m_serverCertRaw, "server");			
		m_clientCert = new X509Certificate2 (m_clientCertRaw, "client");
	}

	[Test] //bug https://bugzilla.novell.com/show_bug.cgi?id=457120
	public void AuthenticateClientAndServer_ClientSendsNoData ()
	{
		AuthenticateClientAndServer (true, true);
	}

	void AuthenticateClientAndServer (bool server, bool client)
	{
		IPEndPoint endPoint = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 10000);
		ClientServerState state = new ClientServerState ();
		state.Client = new TcpClient ();
		state.Listener = new TcpListener (endPoint);
		state.Listener.Start ();
		state.ServerAuthenticated = new AutoResetEvent (false);
		state.ClientAuthenticated = new AutoResetEvent (false);
		state.ServerIOException = !server;
		try {
			Thread serverThread = new Thread (() => StartServerAndAuthenticate (state));
			serverThread.Start (null);
			Thread clientThread = new Thread (() => StartClientAndAuthenticate (state, endPoint));
			clientThread.Start (null);
			Assert.AreEqual (server, state.ServerAuthenticated.WaitOne (TimeSpan.FromSeconds (2)), 
				"server not authenticated");
			Assert.AreEqual (client, state.ClientAuthenticated.WaitOne (TimeSpan.FromSeconds (2)), 
				"client not authenticated");
		} finally {
			if (state.ClientStream != null)
				state.ClientStream.Dispose ();
			state.Client.Close ();
			if (state.ServerStream != null)
				state.ServerStream.Dispose ();
			if (state.ServerClient != null)
				state.ServerClient.Close ();
			state.Listener.Stop ();
		}
	}

	[Test]
	public void ClientCipherSuitesCallback ()
	{
		try {
			ServicePointManager.ClientCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
				return new List<string> { prefix + "RSA_WITH_AES_128_CBC_SHA" };
			};
			// client will only offers AES 128 - that's fine since the server support it (and many more ciphers)
			AuthenticateClientAndServer_ClientSendsNoData ();
		}
		finally {
			ServicePointManager.ClientCipherSuitesCallback = null;
		}
	}

	[Test]
	public void ServerCipherSuitesCallback ()
	{
		try {
			ServicePointManager.ServerCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
					return new List<string> { prefix + "RSA_WITH_AES_256_CBC_SHA" };
			};
			// server only accept AES 256 - that's fine since the client support it (and many more ciphers)
			AuthenticateClientAndServer_ClientSendsNoData ();
		}
		finally {
			ServicePointManager.ServerCipherSuitesCallback = null;
		}
	}

	[Test]
	public void CipherSuitesCallbacks ()
	{
		try {
			ServicePointManager.ClientCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
				return new List<string> { prefix + "RSA_WITH_AES_128_CBC_SHA", prefix + "RSA_WITH_AES_256_CBC_SHA" };
			};
			ServicePointManager.ServerCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
				return new List<string> { prefix + "RSA_WITH_AES_128_CBC_SHA", prefix + "RSA_WITH_AES_256_CBC_SHA" };
			};
			// both client and server supports AES (128 and 256) - server will select 128 (first choice)
			AuthenticateClientAndServer_ClientSendsNoData ();
		}
		finally {
			ServicePointManager.ClientCipherSuitesCallback = null;
			ServicePointManager.ServerCipherSuitesCallback = null;
		}
	}

	[Test]
	public void MismatchedCipherSuites ()
	{
		try {
			ServicePointManager.ClientCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
				return new List<string> { prefix + "RSA_WITH_AES_128_CBC_SHA" };
			};
			ServicePointManager.ServerCipherSuitesCallback += (SecurityProtocolType p, IEnumerable<string> allCiphers) => {
				string prefix = p == SecurityProtocolType.Tls ? "TLS_" : "SSL_";
				return new List<string> { prefix + "RSA_WITH_AES_256_CBC_SHA" };
			};
			// mismatch! server will refuse and send back an alert
			AuthenticateClientAndServer (false, false);
		}
		finally {
			ServicePointManager.ClientCipherSuitesCallback = null;
			ServicePointManager.ServerCipherSuitesCallback = null;
		}
	}

	private void StartClientAndAuthenticate (ClientServerState state, 
						 IPEndPoint endPoint) {
		try {
			state.Client.Connect (endPoint.Address, endPoint.Port);
			NetworkStream s = state.Client.GetStream ();
			state.ClientStream = new SslStream (s, false, 
						(a1, a2, a3, a4) => true,
						(a1, a2, a3, a4, a5) => m_clientCert);
			state.ClientStream.AuthenticateAsClient ("test_host");
			state.ClientAuthenticated.Set ();
		} catch (ObjectDisposedException) { /* this can happen when closing connection it's irrelevant for the test result*/
		} catch (IOException) {
			if (!state.ServerIOException)
				throw;
		}
	}

	private void StartServerAndAuthenticate (ClientServerState state) {
		try {
			state.ServerClient = state.Listener.AcceptTcpClient ();
			NetworkStream s = state.ServerClient.GetStream ();
			state.ServerStream = new SslStream (s, false, 
						(a1, a2, a3, a4) => true, 
						(a1, a2, a3, a4, a5) => m_serverCert);
			state.ServerStream.AuthenticateAsServer (m_serverCert);
			state.ServerAuthenticated.Set ();
		} catch (ObjectDisposedException) { /* this can happen when closing connection it's irrelevant for the test result*/
		} catch (IOException) {
			// The authentication or decryption has failed.
			// ---> Mono.Security.Protocol.Tls.TlsException: Insuficient Security
			// that's fine for MismatchedCipherSuites
			if (!state.ServerIOException)
				throw;
		}
	}
	
	private class ClientServerState {
		public TcpListener Listener { get; set; }
		public TcpClient Client { get; set; }
		public TcpClient ServerClient { get; set; }
		public SslStream ServerStream { get; set; }
		public SslStream ClientStream { get; set; }
		public AutoResetEvent ServerAuthenticated { get; set; }
		public AutoResetEvent ClientAuthenticated { get; set; }
		public bool ServerIOException { get; set; }
	}
}	
}

