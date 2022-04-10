// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

// Contributors (in alphabetical order): [1]
//  * Daniel J. Bernstein
//  * Niels Duif
//  * Tanja Lange,
//  * Peter Schwabe
//  * Bo-Yin Yang
// The Ed25519 software is in the public domain. [2]
// [1] https://ed25519.cr.yp.to/
// [2] https://ed25519.cr.yp.to/software.html

namespace supercop.crypto_sign.ed25519.ref10
{
    /*
    ge means group element.

    Here the group is the set of pairs (x,y) of field elements (see fe.h)
    satisfying -x^2 + y^2 = 1 + d x^2y^2
    where d = -121665/121666.

    Representations:
      ge_p2 (projective): (X:Y:Z) satisfying x=X/Z, y=Y/Z
      ge_p3 (extended): (X:Y:Z:T) satisfying x=X/Z, y=Y/Z, XY=ZT
      ge_p1p1 (completed): ((X:Z),(Y:T)) satisfying x=X/Z, y=Y/T
      ge_precomp (Duif): (y+x,y-x,2dxy)
    */

    internal partial struct ge_p2
    {
        internal fe X;
        internal fe Y;
        internal fe Z;
    }
}
