#pragma once
#include "PluginSettings.h"
#include "PluginClass.h"
#ifdef __cplusplus
extern "C"
{
#endif
	// Put your functions here
	PLUGIN_API int Test();
	PLUGIN_API void LogMetrics(vec3 _position, float _accuracy, float _time);
#ifdef __cplusplus
}
#endif