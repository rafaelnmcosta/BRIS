import React from 'react';
import FormLogin from '../organisms/FormLogin';
import logoUfg from "../../assets/UFG_logo.png";
import logoEvz from "../../assets/EVZ_UFG.svg";
import logoInf from "../../assets/INF_UFG.svg";

const TemplateLogin = ({ handleLogin }) => {
  return (
    <div className="h-full flex">
      {/* Lado esquerdo - informações */}
      <div className="w-2/3 flex flex-col items-start text-green-dark px-16 pt-20">
        <h1 className="text-4xl font-bold mb-4">Bem vindo ao Boar Reproductive Identifier Software!</h1>
        <p className="text-lg mb-8">
        O Boar Reproductive Identifier Software (BRIS) é um software que que tem como objetivo facilitar o processo de classificação e seleção de reprodutores suínos com maior sensibilidade ao resfriamento da dose inseminante. Esta análise é feita de acordo com a metodologia adaptada de Reis (2002) através da análise do Percentual de Motilidade Progressiva (PMP), e é fundamental para evitar a perda de doses inseminantes ao identificar possíveis animais com tendência à sensibilidade ao resfriamento.
        </p>
        <div className="flex gap-4 mt-4 pb-10">
          <img src={logoUfg} alt="Logo UFG" className="h-16" />
          <img src={logoEvz} alt="Logo EVZ" className="h-16" />
          <img src={logoInf} alt="Logo INF" className="h-16" />
        </div>
      </div>

      {/* Lado direito - formulário */}
      <div className="w-1/2 h-full flex pt-12 items-start justify-center">
        <FormLogin handleLogin={handleLogin} />
      </div>
    </div>
  );
};

export default TemplateLogin;
