// src/components/organisms/ResultadoFinal.jsx
import React from "react";
import { Card, Tag, Typography } from "antd";

const { Paragraph, Text } = Typography;

const ResultadoFinal = ({ resultado, status }) => {
  const numericStatus = Number(status) || 1; // status vem como "1"/"2"/"3"
  const isFinalizada = numericStatus !== 1;

  let tag = { color: "default", label: "Não calculado" };

  if (resultado === "True") tag = { color: "green", label: "Aprovado" };
  else if (resultado === "False") tag = { color: "red", label: "Reprovado" };

  return (
    <Card title="Resultado Final" className="rounded-lg shadow-sm">
      <div className="flex flex-col gap-4">
        <Paragraph><Text strong>Estado da avaliação: </Text>{isFinalizada ? <Text>Finalizada</Text> : <Text>Em andamento</Text>}</Paragraph>
        <Paragraph><Text strong>Resultado consolidado: </Text><Tag color={tag.color}>{tag.label}</Tag></Paragraph>
      </div>
    </Card>
  );
};

export default ResultadoFinal;
