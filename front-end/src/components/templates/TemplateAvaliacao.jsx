// src/components/templates/TemplateAvaliacao.jsx
import React from "react";
import { Divider } from "antd";
import HeaderAvaliacao from "../organisms/HeaderAvaliacao";
import ListaSemanas from "../organisms/ListaSemanas";
import ResultadoFinal from "../organisms/ResultadoFinal";

const TemplateAvaliacao = ({
  avaliacao,
  onAtualizar,
  onInterromper,
  onReativar,
  onFinalizar
}) => {
  return (
    <div className="flex flex-col gap-6">
      <HeaderAvaliacao
        avaliacao={avaliacao}
        onInterromper={onInterromper}
        onReativar={onReativar}
        onFinalizar={onFinalizar}
      />

      <Divider />

      <ListaSemanas
        avaliacao={avaliacao}
        onAtualizar={onAtualizar}
      />

      <Divider />

      <ResultadoFinal
        resultado={avaliacao.resultadoFinal}
        status={avaliacao.statusAvaliacao}
      />
    </div>
  );
};

export default TemplateAvaliacao;
