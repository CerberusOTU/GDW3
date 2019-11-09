#include "Wrapper.h"
PluginClass pluginClass;
int Test()
{
	return pluginClass.Test();
};

void LogMetrics(vec3 _position, float _accuracy, float _time)
{
	return pluginClass.LogMetrics(_position, _accuracy, _time);
};
