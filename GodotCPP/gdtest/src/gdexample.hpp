#ifndef GDEXAMPLE_HPP
#define GDEXAMPLE_HPP

#include <Godot.hpp>
#include <Node.hpp>

namespace godot {
	class GDExample : public Node {
		GODOT_CLASS(GDExample, Node)
	private:
		float time_passed;

	};
}
