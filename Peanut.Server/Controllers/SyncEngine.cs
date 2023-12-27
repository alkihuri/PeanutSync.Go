namespace Peanut.Server.Domain; 

public class PredictionUtility
{
    // Статическая функция для предсказания следующей точки на основе массива точек
    public static Position PredictNextPoint(Position[] dataPoints)
    {
        if (dataPoints.Length < 2)
        { 
            Position a = new Position();
            a.x = 0;
            a.y = 0;
            a.z =0;
            return a ;
        }

        float sumX = 0;
        float sumY = 0;
        float sumXY = 0;
        float sumX2 = 0;

        for (int i = 0; i < dataPoints.Length; i++)
        {
            float x = i;
            float y = (float)dataPoints[i].y;

            sumX += x;
            sumY += y;
            sumXY += x * y;
            sumX2 += x * x;
        }

        float n = dataPoints.Length;
        float slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);

        // Предсказываем следующую точку на основе линейной регрессии
        float predictedX = n;
        float predictedY = slope * predictedX + (sumY - slope * sumX) / n;

        // Создаем новую точку с предсказанными значениями
        Position predictedPoint = new Position();

        predictedPoint.x = predictedX;
        predictedPoint.y = predictedY;
        predictedPoint.z = 0;

        return predictedPoint;
    }
}