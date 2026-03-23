import unittest

from triangle_func import get_triangle_type, IncorrectTriangleSides

class TestGetTriangleType(unittest.TestCase):
    """Unit tests for get_triangle_type function using unittest framework."""

    def test_equilateral_triangle(self):
        """Test equilateral triangle (all sides equal)."""
        self.assertEqual(get_triangle_type(5, 5, 5), "equilateral")
        self.assertEqual(get_triangle_type(2, 2, 2), "equilateral")

    def test_isosceles_triangle(self):
        """Test isosceles triangle (two sides equal)."""
        self.assertEqual(get_triangle_type(5, 5, 3), "isosceles")
        self.assertEqual(get_triangle_type(3, 5, 5), "isosceles")
        self.assertEqual(get_triangle_type(5, 3, 5), "isosceles")

    def test_nonequilateral_triangle(self):
        """Test scalene triangle (all sides different)."""
        self.assertEqual(get_triangle_type(3, 4, 5), "nonequilateral")
        self.assertEqual(get_triangle_type(6, 8, 10), "nonequilateral")

    def test_invalid_zero_side(self):
        """Test that zero side raises exception."""
        with self.assertRaises(IncorrectTriangleSides):
            get_triangle_type(0, 1, 1)

    def test_invalid_negative_side(self):
        """Test that negative side raises exception."""
        with self.assertRaises(IncorrectTriangleSides):
            get_triangle_type(-1, 2, 2)

    def test_invalid_triangle_inequality(self):
        """Test that triangle inequality violation raises exception."""
        with self.assertRaises(IncorrectTriangleSides):
            get_triangle_type(1, 2, 3)
        with self.assertRaises(IncorrectTriangleSides):
            get_triangle_type(1, 1, 2)

    def test_invalid_non_numeric(self):
        """Test that non-numeric sides raise exception."""
        with self.assertRaises(IncorrectTriangleSides):
            get_triangle_type("a", 2, 3)

if __name__ == "__main__":
    unittest.main()
