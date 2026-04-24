shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform bool is_intro = true;
uniform vec2 center = vec2(0.5, 0.5);

float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453);
}

float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));
    vec2 u = f * f * (3.0 - 2.0 * f);
    return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

void fragment() {
    vec2 uv = UV - center;
    float dist = length(uv);
    float n = noise(UV * 8.0) * 0.15;

    float fog = 1.0 - smoothstep(progress - n, progress - n + 0.1, dist);
    COLOR = vec4(0.0, 0.0, 0.0, fog);
}
