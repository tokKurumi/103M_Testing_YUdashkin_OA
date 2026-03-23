import unittest
import math

from triangle_class import Triangle, IncorrectTriangleSides

class TestTriangleClass(unittest.TestCase):
    """Unit tests for Triangle class using unittest framework."""

    def test_create_equilateral_triangle(self):
        """Test creating and validating equilateral triangle."""
        t = Triangle(2, 2, 2)
        self.assertEqual(t.triangle_type(), "equilateral")
        self.assertTrue(math.isclose(t.perimeter(), 6.0))

    def test_create_isosceles_triangle(self):
        """Test creating and validating isosceles triangle."""
        t = Triangle(5, 5, 3)
        self.assertEqual(t.triangle_type(), "isosceles")
        self.assertTrue(math.isclose(t.perimeter(), 13.0))

    def test_create_nonequilateral_triangle(self):
        """Test creating and validating scalene triangle."""
        t = Triangle(3, 4, 5)
        self.assertEqual(t.triangle_type(), "nonequilateral")
        self.assertTrue(math.isclose(t.perimeter(), 12.0))

    def test_perimeter_calculation(self):
        """Test perimeter calculation for various triangles."""
        t1 = Triangle(1, 1, 1)
        self.assertTrue(math.isclose(t1.perimeter(), 3.0))

        t2 = Triangle(5, 12, 13)
        self.assertTrue(math.isclose(t2.perimeter(), 30.0))

    def test_invalid_zero_side(self):
        """Test that zero side raises exception on construction."""
        with self.assertRaises(IncorrectTriangleSides):
            Triangle(0, 1, 1)

    def test_invalid_negative_side(self):
        """Test that negative side raises exception on construction."""
        with self.assertRaises(IncorrectTriangleSides):
            Triangle(-1, 2, 2)

    def test_invalid_triangle_inequality(self):
        """Test that triangle inequality violation raises exception."""
        with self.assertRaises(IncorrectTriangleSides):
            Triangle(1, 2, 3)

        with self.assertRaises(IncorrectTriangleSides):
            Triangle(1, 1, 2)

    def test_invalid_non_numeric(self):
        """Test that non-numeric sides raise exception."""
        with self.assertRaises(IncorrectTriangleSides):
            Triangle("a", 2, 3)


if __name__ == "__main__":
    unittest.main()
