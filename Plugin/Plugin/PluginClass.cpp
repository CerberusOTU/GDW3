#include "PluginClass.h"

int PluginClass::Test()
{
	return 100;
};

void PluginClass::LogMetrics(vec3 _position, float _accuracy, float _time)
{
	ofstream MetricsFile;
	MetricsFile.open("Metrics.txt");
	MetricsFile << "Position: " << _position.x << " " << _position.y << " " << _position.z << endl;
	MetricsFile << "Accuracy: " << _accuracy << endl;
	MetricsFile << "Total Time: " << _time << endl;
	MetricsFile.close();
	return;
};
