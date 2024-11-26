import React from 'react';
import FormAutoCadastro from '../organisms/FormAutoCadastro';
import logoUfg from "../../assets/UFG_logo.png";
import logoEvz from "../../assets/EVZ_UFG.svg";
import logoInf from "../../assets/INF_UFG.svg";

const TemplateAutoCadastro = () => {
  return (
    <div className="h-full flex">
      {/* Lado esquerdo - informações */}
      <div className="w-1/2 flex flex-col items-start text-green-dark px-16 pt-12">
        <h1 className="text-4xl font-bold mb-4">Se cadastrando? Saiba os próximos passos!</h1>
        <p className="text-lg mb-8">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer purus arcu, lacinia ultricies nisl et, aliquam iaculis dui. Proin vulputate mollis turpis sed cursus. Curabitur suscipit justo nec vulputate tincidunt. Nulla tincidunt est nibh. Vivamus quis leo non nibh porta imperdiet sed egestas magna. Mauris rhoncus nisi eget purus sodales, ac elementum urna finibus. Etiam dignissim facilisis elit a hendrerit. In eu diam metus. 
        </p>
        <p className="text-green-dark mb-4">
          Já possui cadastro? <span className="text-green-dark">Pode entrar </span>
          <a className="font-bold text-green-dark hover:text-green" href="/login">aqui!</a>
        </p>
        <div className="flex gap-4 mt-4 pb-10">
          <img src={logoUfg} alt="Logo UFG" className="h-16" />
          <img src={logoEvz} alt="Logo EVZ" className="h-16" />
          <img src={logoInf} alt="Logo INF" className="h-16" />
        </div>
      </div>

      {/* Lado direito - formulário */}
      <div className="w-1/2">
        <FormAutoCadastro />
      </div>
    </div>
  );
};

export default TemplateAutoCadastro;
