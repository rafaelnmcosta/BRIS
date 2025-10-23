// src/components/organisms/ListaSemanas.jsx
import React from "react";
import { Collapse, Empty, Tag } from "antd";
import DoseCard from "../molecules/DoseCard";

const { Panel } = Collapse;

const resultadoTagMap = {
  "-1": { color: "default", label: "Não gerado" },
  0: { color: "red", label: "Erro" },
  1: { color: "blue", label: "Menor" },
  2: { color: "orange", label: "Médio" },
  3: { color: "green", label: "Maior" },
};

const ListaSemanas = ({ avaliacao, onAtualizar }) => {
  if (!avaliacao || !avaliacao.semanas || avaliacao.semanas.length === 0) {
    return <Empty description="Nenhuma semana cadastrada para esta avaliação." />;
  }

  return (
    <Collapse accordion bordered style={{ background: "#fff" }}>
      {avaliacao.semanas
        .sort((a, b) => a.nroSemana - b.nroSemana)
        .map((semana) => {
          const resultado = resultadoTagMap[semana.resultado] || {
            color: "default",
            label: "—",
          };

          return (
            <Panel
              key={semana.id}
              header={`Semana ${semana.nroSemana}`}
              extra={<Tag color={resultado.color}>{resultado.label}</Tag>}
            >
              {semana.doses && semana.doses.length > 0 ? (
                <div style={{ display: "flex", flexWrap: "wrap", gap: "16px" }}>
                  {semana.doses.map((dose) => (
                    <DoseCard
                      key={dose.id}
                      avaliacaoId={avaliacao.id}
                      dose={dose}
                      onDoseRegistrada={onAtualizar}
                    />
                  ))}
                </div>
              ) : (
                <Empty
                  description="Não há doses para esta semana"
                  image={Empty.PRESENTED_IMAGE_SIMPLE}
                />
              )}
            </Panel>
          );
        })}
    </Collapse>
  );
};

export default ListaSemanas;
