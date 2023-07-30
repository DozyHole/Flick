using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Line = System.Tuple<UnityEngine.Vector2, UnityEngine.Vector2>;

public class ScrCollision : MonoBehaviour
{
    public static bool PaddleBallCollision(Vector2 ballPosLast, Vector2 ballPosCurrent, Vector2 paddlePosLast, Vector2 paddlePosCurrent, float paddleLength, float paddleAngleLast, float paddleAngleCurrent, ref Vector2 hitPointOut)
    {
        // ccd between ball and paddle,
        // ball (point) sweeps out a straght line between frames,
        // paddle (2D line) sweeps out a 4 sided polygon between frames

        // get ball travel line
        Line ball = new Line(ballPosLast, ballPosCurrent);

        // calculate 4 points of paddle quad
        float angleLastRad = Mathf.Deg2Rad * paddleAngleLast;
        float angleCurrentRad = Mathf.Deg2Rad * paddleAngleCurrent;
        Vector2 leftStart = paddlePosLast + RotatePoint(new Vector2(-paddleLength / 2.0f, 0.0f), -angleLastRad);
        Vector2 leftEnd = paddlePosCurrent + RotatePoint(new Vector2(-paddleLength / 2.0f, 0.0f), -angleCurrentRad);
        Vector2 rightStart = paddlePosLast + RotatePoint(new Vector2(paddleLength / 2.0f, 0.0f), -angleLastRad);
        Vector2 rightEnd = paddlePosCurrent + RotatePoint(new Vector2(paddleLength / 2.0f, 0.0f), -angleCurrentRad);

        // hit detect 4 points
        List<Line> sides  = new List<Line>();
        sides.Add(new Line(leftStart, leftEnd));    // bottom
        sides.Add(new Line(rightStart, rightEnd));  // top
        sides.Add(new Line(leftStart, rightStart)); // right
        sides.Add(new Line(leftEnd, rightEnd));     // top
        // debug view
            Debug.DrawLine(leftStart, leftEnd, Color.green);
            Debug.DrawLine(rightStart, rightEnd, Color.green);
            Debug.DrawLine(leftStart, rightStart, Color.green);
            Debug.DrawLine(leftEnd, rightEnd, Color.green);
        bool hit = LinePolygonIntersection(ball, sides, ref hitPointOut);
        return hit;
    }

    /*
        Return true if at least one side intersects with the given line p
    */
    public static bool LinePolygonIntersection(Line p, List<Line> sides, ref Vector2 hitPointOut)
    {
        bool intersect = false;
        Vector2 hitPoint = new Vector2();
        sides.ForEach(side => {
            if (LineLineIntersect(p.Item1, p.Item2, side.Item1, side.Item2)){
                intersect = true;
                // get hit point
                Vector2 p1 = p.Item1;
                Vector2 q1 = p.Item2;
                Vector2 p2 = side.Item1;
                Vector2 q2 = side.Item2;
                hitPoint.x = ((p1.x * q1.y - p1.y * q1.x) * (p2.x - q2.x) - (p2.x * q2.y - p2.y * q2.x) * (p1.x - q1.x)) / ((p1.x - q1.x) * (p2.y - q2.y) - (p1.y - q1.y) * (p2.x - q2.x));
                hitPoint.y = ((p1.x * q1.y - p1.y * q1.x) * (p2.y - q2.y) - (p2.x * q2.y - p2.y * q2.x) * (p1.y - q1.y)) / ((p1.x - q1.x) * (p2.y - q2.y) - (p1.y - q1.y) * (p2.x - q2.x));

            }
        });
        hitPointOut = hitPoint;
        return intersect;
    }

    /*
        Line - line intersection without intesection point
        https://www.codingninjas.com/studio/library/check-if-two-line-segments-intersect
    */
    public static bool LineLineIntersect(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
    {
        // Compute the directions of the four line segments
        float d1 = Direction(a1, b1, a2);
        float d2 = Direction(a1, b1, b2);
        float d3 = Direction(a2, b2, a1);
        float d4 = Direction(a2, b2, b1);

        // Check if the two line segments intersect
        if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) && ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0)))
        {
            return true;
        }

        // Check if the line segments are collinear and overlapping
        if (AreCollinearAndOverlapping(a1, b1, a2, b2) || AreCollinearAndOverlapping(a2, b2, a1, b1))
        {
            return true;
        }

        return false;
    }

    // better version?
    // https://www.jeffreythompson.org/collision-detection/poly-line.php
    // LINE/LINE
    //boolean lineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    //{

    //    // calculate the direction of the lines
    //    float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
    //    float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

    //    // if uA and uB are between 0-1, lines are colliding
    //    if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    // helper functions //

    /* 
       Computes the direction of the three given points
       Returns a positive value if they form a counter-clockwise orientation,
       a negative value if they form a clockwise orientation,
       and zero if they are collinear 
    */
    private static float Direction(Vector2 p, Vector2 q, Vector2 r)
    {
        return (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
    }

    // Checks if two line segments are collinear and overlapping
    private static bool AreCollinearAndOverlapping(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
    {
        // Check if the line segments are collinear
        if (Direction(a1, b1, a2) == 0.0f)
        {
            // Check if the line segments overlap
            if (a2.x <= Mathf.Max(a1.x, b1.x) && a2.x >= Mathf.Min(a1.x, b1.x) && a2.y <= Mathf.Max(a1.y, b1.y) && a2.y >= Mathf.Min(a1.y, b1.y))
            {
                return true;
            }
        }
        return false;
    }

    /*
     * Rotate point by some degrees about the origin (radians)
     */
    private static Vector2 RotatePoint(Vector2 p, float angle)
    {
        //𝑢=𝑠cos𝜃+𝑡sin𝜃and𝑣=−𝑠sin𝜃+𝑡cos𝜃.
        float u = p.x*Mathf.Cos(angle) + p.y*Mathf.Sin(angle);
        float v = -p.x * Mathf.Sin(angle) + p.y * Mathf.Cos(angle);
        return new Vector2(u, v);
    }

}
