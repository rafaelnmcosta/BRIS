// src/components/molecules/DoseCard.jsx
import React, { useState } from "react";
import { Card, Button, Modal, InputNumber, message, Tag } from "antd";
import { avaliacoes } from "../../api/avaliacoesAPI";

const DoseCard = ({ avaliacaoId, dose, onDoseRegistrada }) => {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [valorRegistrado, setValorRegistrado] = useState(dose.valorRegistrado ?? null);
  const [loading, setLoading] = useState(false);

  const abrirModal = () => setIsModalVisible(true);
  const fecharModal = () => {
    setIsModalVisible(false);
    setValorRegistrado(dose.valorRegistrado ?? null);
  };

  const registrarDose = async () => {
    if (valorRegistrado === null || valorRegistrado === undefined) {
      message.warning("Por favor, insira um valor para a dose.");
      return;
    }

    try {
      setLoading(true);
      const payload = { valorRegistrado: valorRegistrado };
      await avaliacoes.novaDose(avaliacaoId, payload);
      message.success("Dose registrada com sucesso!");
      fecharModal();
      if (onDoseRegistrada) onDoseRegistrada();
    } catch (error) {
      const errMsg =
        error.response?.data?.message ||
        error.response?.data ||
        error.message ||
        "Erro ao registrar dose.";
      message.error(errMsg);
    } finally {
      setLoading(false);
    }
  };

  const getStatusTag = () => {
    if (dose.valorRegistrado !== null && dose.valorRegistrado !== undefined) {
      return <Tag color="green">Preenchida</Tag>;
    }
    if (!dose.podePreencher) {
      return <Tag color="default">Bloqueada</Tag>;
    }
    return <Tag color="blue">Pendente</Tag>;
  };

  return (
    <>
      <Card title={`Dose ${dose.ordem}`} style={{ width: 250, margin: "10px" }} actions={
        dose.podePreencher ? [<Button type="primary" onClick={abrirModal}>Registrar Dose</Button>] : []
      }>
        <p><strong>Status:</strong> {getStatusTag()}</p>
        <p><strong>Valor:</strong> {dose.valorRegistrado ?? "—"}</p>
      </Card>

      <Modal
        title={`Registrar Dose ${dose.ordem}`}
        open={isModalVisible}
        onOk={registrarDose}
        onCancel={fecharModal}
        confirmLoading={loading}
        okText="Registrar"
        cancelText="Cancelar"
      >
        <p>Informe o valor registrado:</p>
        <InputNumber
          style={{ width: "100%" }}
          min={0}
          step={0.1}
          precision={2}
          value={valorRegistrado}
          onChange={(value) => setValorRegistrado(value)}
        />
      </Modal>
    </>
  );
};

export default DoseCard;
