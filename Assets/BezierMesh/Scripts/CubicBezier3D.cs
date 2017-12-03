using System;
using UnityEngine;

public struct CubicBezier3D {
    public Vector3[] points;

    public CubicBezier3D(Vector3[] points) {
        if(points.Length != 4)  throw new Exception("Not 4 points");

        this.points = new Vector3[points.Length];
        for(int i = 0; i < points.Length; i++) {
            this.points[i] = points[i];
        }
    }

    public Vector3 GetPoint( float t ) {
        float omt = 1f-t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return points[0] * ( omt2 * omt ) +
            points[1] * ( 3f * omt2 * t ) +
            points[2] * ( 3f * omt * t2 ) +
            points[3] * ( t2 * t );
    }

    public Vector3 GetTangent( float t ) {
        float omt = 1f-t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
            points[0] * ( -omt2 ) +
            points[1] * ( 3 * omt2 - 2 * omt ) +
            points[2] * ( -3 * t2 + 2 * t ) +
            points[3] * ( t2 );
        return tangent.normalized;
    }

    public Vector3 GetNormal2D( float t ) {
        Vector3 tng = GetTangent( t );
        return new Vector3( -tng.y, tng.x, 0f );
    }

    public Vector3 GetNormal3D( float t, Vector3 up ) {
        Vector3 tng = GetTangent( t );
        Vector3 binormal = Vector3.Cross( up, tng ).normalized;
        return Vector3.Cross( tng, binormal );
    }

    public Quaternion GetOrientation2D( float t ) {
        Vector3 tng = GetTangent( t );
        Vector3 nrm = GetNormal2D( t );
        return Quaternion.LookRotation( tng, nrm );
    }

    public Quaternion GetOrientation3D( float t, Vector3 up ) {
        Vector3 tng = GetTangent( t );
        Vector3 nrm = GetNormal3D( t, up );
        return Quaternion.LookRotation( tng, nrm );
    }
};
